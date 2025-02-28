using AOSync.BL.ApiClient;
using AOSync.BL.ProcessingModules;
using AOSync.BL.Services.Synchronization.Interfaces;
using AOSync.COMMON.Models;
using AOSync.DAL.Entities;
using AOSync.DAL.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AOSync.BL.Services.Synchronization;

public class SynchronizationService : ISynchronizationService
{
    private readonly IConfiguration _configuration;
    private readonly IServiceProvider _serviceProvider;
    private readonly ISynchronizationApiClient _apiClient;
    private readonly ITransactionService _transactionService;
    private readonly ILogger<SynchronizationService> _logger;
    private readonly SyncGetChangesResultProcessor _syncGetChangesResultProcessor;
    private readonly SyncGetInitialChangesResultProcessor _syncGetInitialChangesResultProcessor;
    
    private readonly SyncSetExternals _externals = new();

    public SynchronizationService(
        IConfiguration configuration,
        IServiceProvider serviceProvider,
        ILogger<SynchronizationService> logger,
        ISynchronizationApiClient apiClient,
        ITransactionService transactionService)
    {
        var profile = configuration.GetValue<string>("ConfigurationProfile")!;
        _configuration = configuration.GetSection(profile);
        _serviceProvider = serviceProvider;
        _apiClient = apiClient;
        _transactionService = transactionService;
        _logger = logger;
        _syncGetChangesResultProcessor = _serviceProvider.GetRequiredService<SyncGetChangesResultProcessor>();
        _syncGetInitialChangesResultProcessor =
            _serviceProvider.GetRequiredService<SyncGetInitialChangesResultProcessor>();

        _syncGetChangesResultProcessor.Initialize(_serviceProvider, _externals);
        _syncGetInitialChangesResultProcessor.Initialize(_serviceProvider, _externals);
    }

    public async Task SetChanges()
    {
        // using var scope = _serviceProvider.CreateScope();
        // var changeTrackingService = scope.ServiceProvider.GetRequiredService<IChangeTrackingRepository>();
        // var createdEntities = await changeTrackingService.GetCreatedRecordsAsync();
        // var converter = new EntityToApiClassConverter(_serviceProvider, _configuration);
        // var changes = await converter.CreatedEntitiesToApiClassAsync(createdEntities);
        //
        // var syncSetChanges = new SyncSetChanges
        // {
        //     Company = _configuration.GetValue<string>("CompanyID"),
        //     Usercompany = _configuration.GetValue<string>("UserCompanyID"),
        //     Changes = changes
        // };
        //
        // await _apiClient.SyncSetChangesAsync(syncSetChanges);
    }

    public async Task GetChanges(string lastTranId)
    {
        try
        {
            SyncGetChangesResult result;
            do
            {
                result = await _apiClient.SyncGetChangesAsync(new SyncGetChanges
                {
                    Company = _configuration.GetValue<string>("CompanyID"),
                    SimpleResult = false,
                    LasttranId = lastTranId
                });

                if (result.Iserror == true && result.Isrepeatable != true)
                    throw new InvalidDataException("Error during GetChanges()");

            } while (result.Isrepeatable == true);

            await ProcessChanges(result.Trans);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetChanges");
        }
    }

    private async Task ProcessChanges(ICollection<SyncGetTransaction> transactions)
    {
        foreach (var transaction in transactions)
        {
            if (transaction.Changes?.Count > 0)
                await SyncGetChangesResultProcessor.HandleComponents(transaction.Changes);
        }
    }

    public async Task<string> GetLastTransactionId()
    {
        var lastTranId = await _transactionService.GetLatestTransactionId();
        if (!string.IsNullOrEmpty(lastTranId)) return lastTranId;

        var result = await _apiClient.SyncGetCurrentTranidAsync(new SyncGetCurrentTranid
        {
            Company = _configuration.GetValue<string>("CompanyID")
        });

        return result.Tranid;
    }

    public async Task GetInitialChanges(string lastTranId)
    {
        var initialChangesResult = await _apiClient.SyncGetInitialChangesAsync(new SyncGetInitialChanges
        {
            Company = _configuration.GetValue<string>("CompanyID"),
            Maxtranid = lastTranId,
            Withexternalid = _configuration.GetValue<bool>("Withexternalid")
        });

        await SyncGetInitialChangesResultProcessor.HandleComponents(initialChangesResult.Components);
    }

    public async Task<bool> StoreTransactionIdAsync(string tranId)
    {
        return await _transactionService.AddOrUpdateAsync(new TransactionEntity
        {
            Id = tranId,
            DateAdded = DateTime.Now
        }) != null;
    }
}