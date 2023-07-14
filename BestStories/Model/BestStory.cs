namespace BestStories.Model
{
    public class BestStory
    {
        public string title { get; set; }
        public string uri { get; set; }
        public string postedBy { get; set; }
        public DateTime time { get; set; }
        public int score { get; set; }
        public int commentCount { get; set; }
    }


    public class RawData
    {
        public string by { get; set; }
        public int descendants { get; set; }
        public int id { get; set; }
        public int[] kids { get; set; }
        public int score { get; set; }
        public int time { get; set; }
        public string title { get; set; }
        public string type { get; set; }
        public string url { get; set; }
    }

}
