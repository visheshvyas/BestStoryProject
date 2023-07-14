using Newtonsoft.Json;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Xml;

namespace BestStoriesClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("How many stories you want to fetch : ");
            _ = int.TryParse(Console.ReadLine(), out int requestCnt);

            while (requestCnt > 0)
            {
                Console.WriteLine($"Fetching {requestCnt} BestStories...");
                try
                {
                    using var client = new HttpClient();
                    using var s = client.GetStringAsync($"https://localhost:7219/BestStory?cnt={requestCnt}");
                    if (!string.IsNullOrEmpty(s.Result))
                    {
                        dynamic storyData = JsonConvert.DeserializeObject(s.Result);
                        if (storyData != null)
                        {
                            Console.Write($"For Count : {requestCnt}\n {JsonConvert.SerializeObject(storyData, Newtonsoft.Json.Formatting.Indented)}\n\n\n");
                        }
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.InnerException?.Message + "\nPlease try again in some time...\n");
                }

                Console.WriteLine("How many stories you want to fetch : ");
                _ = int.TryParse(Console.ReadLine(), out requestCnt);
            }

            Console.WriteLine("Press enter to exit.");
            Console.ReadLine();
        }
    }
}