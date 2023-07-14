using BestStories.Interface;

namespace BestStories.Helper
{
    public class InitializeService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        
        public InitializeService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            //this is to initialise StoryRepository
            //so that data is available for clients to fetch
            _serviceProvider.GetService<IStoryRepository>();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
