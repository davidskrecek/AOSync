using AOSync.BL.Services;
using AOSync.BL.Services.Synchronization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using AOSync.APICLIENT;
using AOSync.BL.Services.Synchronization.Interfaces;
using Microsoft.Extensions.DependencyInjection;
//using SyncGetCurrentTranid = AOSync.COMMON.Models.SyncGetCurrentTranid;

namespace AOSync.BL
{
    public class SyncBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfigurationSection _configuration;
        private readonly ISynchronizationService _synchronizationService;
        private readonly ILogger<SyncBackgroundService> _logger;
        private readonly ISynchronizationApiClient _synchronizationApiClient;

        private string _lastTranId { get; set; } = string.Empty;

        public SyncBackgroundService(IConfigurationSection configuration, ILogger<SyncBackgroundService> logger, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
            _synchronizationService = _serviceProvider.GetRequiredService<ISynchronizationService>();
            _logger = logger;
            _synchronizationApiClient = _serviceProvider.GetRequiredService<ISynchronizationApiClient>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Run a background task that periodically performs sync operations
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    bool initialize = _configuration.GetValue<bool>("SystemInitialization");
                    // Initial sync setup
                    if (_configuration.GetValue<bool>("SystemInitialization"))
                    {
                        // Initialize synchronization
                        await InitializeSync();
                    }

                    await InitializeSync();

                    // Perform periodic sync every 5 minutes
                    var timer = new Timer(async _ =>
                    {
                        try
                        {
                            await PerformPeriodicSync();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error during periodic sync.");
                        }
                    }, null, TimeSpan.Zero, TimeSpan.FromSeconds(5)); // Timer runs immediately and repeats every 5 minutes

                    // Block to keep the background service alive until cancellation
                    await Task.Delay(Timeout.Infinite, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in background service.");
                }
            }
        }

        private async Task InitializeSync()
        {
            try
            {
                var request = new SyncGetCurrentTranid()
                {
                    Company = _configuration.GetValue<string>("Company"),
                };

                // Retry logic for getting the transaction ID
                var response = await _synchronizationApiClient.SyncGetCurrentTranidAsync(request);
                // TODO RESOLVE THIS ERROR
                // fail: AOSync.BL.SyncBackgroundService[0]
                // Error during initial sync setup.
                //     Newtonsoft.Json.JsonSerializationException: Cannot write a null value for property 'company'. Property requires a value. Path ''.
                if (response.Iserror == true)
                {
                    if (response.Isrepeatable == true)
                    {
                        // Retry the request if it's repeatable
                        _logger.LogWarning("Transaction ID fetch failed, retrying...");
                        return;
                    }

                    throw new InvalidOperationException(response.Error); // Component Company not found
                }

                _lastTranId = response.Tranid;
                Console.WriteLine(_lastTranId);

                // Get initial changes and store the transaction ID
                await _synchronizationService.GetInitialChanges(_lastTranId);
                await _synchronizationService.StoreTransactionIdAsync(_lastTranId);

                _logger.LogInformation("Initial sync setup completed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during initial sync setup.");
            }
        }

        private async Task PerformPeriodicSync()
        {
            try
            {
                // Fetch the last transaction ID and sync changes
                _lastTranId = await _synchronizationService.GetLastTransactionId();
                await _synchronizationService.GetChanges(_lastTranId);
                //await _synchronizationService.SetChanges();

                _logger.LogInformation("Periodic sync completed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during periodic sync.");
            }
        }
    }
}
