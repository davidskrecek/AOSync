using AOSync.BL.ApiClient;
using AOSync.BL.Services;
using AOSync.BL.Services.Synchronization;
using AOSync.COMMON.Models;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AOSync.BL
{
    public class SyncBackgroundService : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly SynchronizationService _synchronizationService;
        private readonly ILogger<SyncBackgroundService> _logger;
        private readonly ISynchronizationApiClient _synchronizationApiClient;

        private string _lastTranId { get; set; } = string.Empty;

        public SyncBackgroundService(IConfiguration configuration, SynchronizationService synchronizationService, ILogger<SyncBackgroundService> logger, ISynchronizationApiClient synchronizationApiClient)
        {
            _configuration = configuration;
            _synchronizationService = synchronizationService;
            _logger = logger;
            _synchronizationApiClient = synchronizationApiClient;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Perform initial sync
            try
            {
                if (_configuration.GetValue<bool>("SystemInitialization"))
                {
                    var request = new SyncGetCurrentTranid()
                    {
                        Company = _configuration.GetValue<string>("CompanyId")
                    };
                    request:
                    var response = await _synchronizationApiClient.SyncGetCurrentTranidAsync(request);
                    if (response.Iserror == true)
                    {
                        if (response.Isrepeatable == true)
                        {
                            goto request;
                        }

                        throw new InvalidOperationException("Failed to fetch transaction ID.");
                    }
                    _lastTranId = response.Tranid;
                    await _synchronizationService.GetInitialChanges(_lastTranId);
                    await _synchronizationService.StoreTransactionIdAsync(_lastTranId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during initial sync");
            }
            // Start periodic sync every 5 seconds
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _lastTranId = await _synchronizationService.GetLastTransactionId();
                    await _synchronizationService.GetChanges(_lastTranId);

                    await _synchronizationService.SetChanges();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during periodic sync");
                }

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }
}