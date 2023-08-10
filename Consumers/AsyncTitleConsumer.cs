using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using CCOM.AsyncTitleServiceConsumer.Contracts;
using CCOM.AsyncTitleServiceConsumer.Services;

namespace CCOM.AsyncTitleServiceConsumer.Consumers
{
    public class AsyncTitleConsumer : IConsumer<AysncTitleProviderMessage>
    {
        private readonly ILogger<AysncTitleProviderMessage> _logger;
        private readonly ITitleAsyncService _titleAsyncService;

        public AsyncTitleConsumer(ILogger<AysncTitleProviderMessage> logger, ITitleAsyncService titleAsyncService)
        {
            _logger = logger;
            _titleAsyncService = titleAsyncService;
        }

        public Task Consume(ConsumeContext<AysncTitleProviderMessage> context)
        {
            var timer = new Stopwatch();
            _logger.LogInformation("We have a new message. MessageType: .",context.Message.MessageType);

            timer.Start();
            _titleAsyncService.Handle(context.Message);
            timer.Stop();

            _logger.LogInformation("Message processed in {time} minutes.", timer.Elapsed.ToString(@"mm\:ss\.fff"));

            return Task.CompletedTask;
        }
    }
}
