using BestStories.Model;

namespace BestStories.Interface
{
    public interface IStoryRepository
    {
        public IEnumerable<BestStory> GetStories(int cnt);
    }
}
