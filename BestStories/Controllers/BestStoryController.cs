using BestStories.Interface;
using BestStories.Model;
using Microsoft.AspNetCore.Mvc;

namespace BestStories.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BestStoryController : ControllerBase
    {
        IStoryRepository _storyRepository;
        public BestStoryController(IStoryRepository storyRepository)
        {
            _storyRepository = storyRepository;
        }

        [HttpGet(Name = "GetBestStoreis")]
        public IEnumerable<BestStory> Get(int cnt)
        {
            return _storyRepository.GetStories(cnt).ToArray();
        }
    }
}
