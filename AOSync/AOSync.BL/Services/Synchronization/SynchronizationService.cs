using System.Runtime.InteropServices.JavaScript;
using AOSync.APICLIENT;
using AOSync.BL.ProcessingModules;
using AOSync.BL.Services.Synchronization.Interfaces;
using AOSync.DAL.Entities;
using AOSync.DAL.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pomelo.EntityFrameworkCore.MySql.Query.ExpressionTranslators.Internal;
// using SyncGetChanges = AOSync.COMMON.Models.SyncGetChanges;
// using SyncGetChangesResult = AOSync.COMMON.Models.SyncGetChangesResult;
// using SyncGetCurrentTranid = AOSync.COMMON.Models.SyncGetCurrentTranid;
// using SyncGetInitialChanges = AOSync.COMMON.Models.SyncGetInitialChanges;
// using SyncGetTransaction = AOSync.COMMON.Models.SyncGetTransaction;
// using SyncSetExternals = AOSync.COMMON.Models.SyncSetExternals;

namespace AOSync.BL.Services.Synchronization;

public class SynchronizationService : ISynchronizationService
{
    private readonly IConfigurationSection _configuration;
    private readonly IServiceProvider _serviceProvider;
    private readonly ISynchronizationApiClient _apiClient;
    private readonly ITransactionRepository _transactionRepository;
    private readonly ILogger<SynchronizationService> _logger;
    private readonly SyncGetChangesResultProcessor _syncGetChangesResultProcessor;
    private readonly SyncGetInitialChangesResultProcessor _syncGetInitialChangesResultProcessor;
    
    private readonly SyncSetExternals _externals = new();

    public SynchronizationService(
        IConfigurationSection configuration,
        IServiceProvider serviceProvider,
        ILogger<SynchronizationService> logger,
        ISynchronizationApiClient apiClient,
        ITransactionRepository transactionRepository)
    {
        _configuration = configuration;
        _serviceProvider = serviceProvider;
        _apiClient = apiClient;
        _transactionRepository = transactionRepository;
        _logger = logger;
        _syncGetChangesResultProcessor = _serviceProvider.GetRequiredService<SyncGetChangesResultProcessor>();
        _syncGetInitialChangesResultProcessor =
            _serviceProvider.GetRequiredService<SyncGetInitialChangesResultProcessor>();

        _syncGetChangesResultProcessor.Initialize(_serviceProvider, _externals);
        _syncGetInitialChangesResultProcessor.Initialize(_serviceProvider, _configuration);
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
                    Company = _configuration.GetValue<string>("Company"),
                    SimpleResult = false,
                    LasttranId = lastTranId,
                    AdditionalProperties =
                        new Dictionary<string, object>(){
                            { "withexternalids", _configuration.GetValue<bool>("WithExternalIds") }
                        },
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
                await _syncGetChangesResultProcessor.HandleComponents(transaction.Changes);
        }
    }

    public async Task<string> GetLastTransactionId()
    {
        var lastTranId = await _transactionRepository.GetLatestTransactionId();
        if (!string.IsNullOrEmpty(lastTranId)) return lastTranId;

        var request = new SyncGetCurrentTranid
        {
            Company = _configuration.GetValue<string>("Company")
        };

        var result = await _apiClient.SyncGetCurrentTranidAsync(request);

        return result.Tranid;
    }

    public async Task GetInitialChanges(string lastTranId)
    {
        SyncGetInitialChanges request = new()
        {
            Company = _configuration.GetValue<string>("Company"),
            Maxtranid = lastTranId,
            Withexternalid = _configuration.GetValue<bool>("Withexternalid")
        };

        do
        {
            var initialChangesResult = await _apiClient.SyncGetInitialChangesAsync(request);
            
            SyncSetExternals syncSetExternals = await _syncGetInitialChangesResultProcessor.HandleComponents(initialChangesResult.Components);
            // TODO nic se neulozi do databaze musim zjistit proc
            break;
            // var syncSetExternalsResult = await _apiClient.SyncSetExternalsAsync(syncSetExternals);
            //
            // if (syncSetExternalsResult.Iserror == false)
            // {
            //     if (syncSetExternalsResult.Isrepeatable != false)
            //     {
            //         Console.WriteLine($"The SynSetExternals finished with a message of \"{syncSetExternalsResult.Error}\"");
            //         break;
            //     }
            // }
        } while (true);
    }

    public async Task<bool> StoreTransactionIdAsync(string tranId)
    {
        return await _transactionRepository.AddOrUpdateAsync(new TransactionEntity
        {
            Id = tranId,
            DateAdded = DateTime.Now
        }) != null!;
    }
}