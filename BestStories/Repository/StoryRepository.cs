using BestStories.Interface;
using BestStories.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using System.Text.Json;

namespace BestStories.Repository
{
    public class StoryRepository : IStoryRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        List<BestStory> lstStories = new List<BestStory>();
        public StoryRepository(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            Console.WriteLine($"***** Starting Service *****");
            Console.WriteLine($"Started loading Story data :: {DateTime.Now.ToString()}");

            string storyids_url = Helper.ConfigurationManager.AppSetting["StoryIdsUrl"];
            int[] storyIds;

            //Fetching all the story ids
            var client = _httpClientFactory.CreateClient();
            using (var s = client.GetStringAsync(storyids_url))
            {
                //as values are comma separated no need to create a separate class
                //int[] will solve the purpose
                storyIds = JsonSerializer.Deserialize<int[]>(s.Result)!;
            }

            ConcurrentBag<BestStory> bagStories = new ConcurrentBag<BestStory>();
            //Fetching all the stories based on the Ids available
            if (storyIds.Length > 0)
            {
                bool loadingCompleted = false;
                
                Task.Factory.StartNew(() => {
                    while (!loadingCompleted)
                    {
                        Console.Write(".");
                        Thread.Sleep(2000);
                    }
                });

                try
                {
                    Parallel.ForEach(storyIds, storyId =>
                    {
                        string story_url = Helper.ConfigurationManager.AppSetting["StoryUrl"].Replace("{story_id}", storyId.ToString());

                        //using (var client = new HttpClient())
                        //{
                        var client = _httpClientFactory.CreateClient();
                        using (var s = client.GetStringAsync(story_url))
                        {
                            RawData rawData = JsonSerializer.Deserialize<RawData>(s.Result)!;
                            if (rawData != null)
                            {
                                //we will only consider type == STORY here
                                if (rawData.type.ToUpper() == "STORY")
                                {
                                    BestStory bestStory = new BestStory()
                                    {
                                        title = rawData.title,
                                        uri = rawData.url,
                                        postedBy = rawData.by,
                                        time = DateTimeOffset.FromUnixTimeSeconds(rawData.time).UtcDateTime,
                                        score = rawData.score,
                                        commentCount = GetCommentCount(rawData.kids),
                                    };
                                    bagStories.Add(bestStory);
                                }
                            }
                        }
                        //}
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    loadingCompleted = true;
                }
            }

            //finally getting the Stories ready with the sorting based on score
            lstStories = bagStories.OrderByDescending(x => x.score).ToList();

            Console.WriteLine($"\nFinished loading Story data :: {DateTime.Now.ToString()}");
            Console.WriteLine($"***** Service Started *****");
        }

        private int GetCommentCount(int[] kids)
        {
            int commentCount = 0;
            Parallel.ForEach(kids, kidId =>
            {
                string story_url = Helper.ConfigurationManager.AppSetting["StoryUrl"].Replace("{story_id}", kidId.ToString());

                var client = _httpClientFactory.CreateClient();
                using (var s = client.GetStringAsync(story_url))
                {
                    RawData rawData = JsonSerializer.Deserialize<RawData>(s.Result)!;
                    if (rawData != null)
                    {
                        //we will only consider type == COMMENT here
                        if (rawData.type.ToUpper() == "COMMENT")
                        {
                            Interlocked.Increment(ref commentCount);
                        }
                    }
                }
            });
            return commentCount;
        }

        public IEnumerable<BestStory> GetStories(int cnt)
        {
            Console.WriteLine($"{DateTime.Now} - Received Request on thread id : {Thread.CurrentThread.ManagedThreadId}");
            return lstStories.Take(cnt);
        }

    }
}
