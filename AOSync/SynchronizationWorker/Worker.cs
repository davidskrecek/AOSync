using AOSync;

namespace SynchronizationWorker
{
    public class Worker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<Worker> _logger;

        public Worker(IServiceProvider serviceProvider, ILogger<Worker> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker service is starting.");

            using var scope = _serviceProvider.CreateScope();
            var apiCommunicator = scope.ServiceProvider.GetRequiredService<ApiCommunicator>();
            var program = scope.ServiceProvider.GetRequiredService<AOSync.Program>();

            await program.InitializeAsync();

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker service is running.");
                // await apiCommunicator.SyncGetChanges(program.LastTransId);
                // await program.StoreTransactionId();
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }

            _logger.LogInformation("Worker service is stopping.");
        }
    }
}