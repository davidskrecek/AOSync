// using System.Diagnostics;
// using AOSync.COMMON.Converters;
// using AOSync.COMMON.Models;
// using AOSync.DAL.DB;
// using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.DependencyInjection;
//
// namespace AOSync.APICLIENT;
//
// public class ApiCommunicator
// {
//     private readonly string _companyId;
//     private readonly string _userCompanyId;
//     private readonly bool _withExternalId = true;
//
//     public ApiCommunicator(IServiceProvider serviceProvider, IConfiguration configuration)
//     {
//         _serviceProvider = serviceProvider;
//         _configuration = configuration;
//         _companyId = _configuration["CompanyId"]!;
//         _userCompanyId = _configuration["UserCompanyId"]!;
//         _httpClientExtension = new HttpClientExtension(_configuration);
//     }
//
//     private string? _lastTranId { get; set; }
//     private IServiceProvider _serviceProvider { get; }
//     private IConfiguration _configuration { get; }
//     private HttpClientExtension _httpClientExtension { get; }
//
//     public static Uri ApiUrl { get; } = new("https://aliteobetaapi.azurewebsites.net");
//
//     public async Task SyncSetExternals(List<SyncExternal> externals)
//     {
//         var repeatable = false;
//         var maxRetries = 5;
//         var retries = 0;
//
//         do
//         {
//             try
//             {
//                 var request = new syncSetExternals
//                 {
//                     company = _companyId,
//                     externals = externals
//                 };
//
//                 var response = await _httpClientExtension.SendRequestAsync<syncSetExternals, syncSetExternalsResult>(
//                     ApiUrl + "/SyncSetExternals", request);
//
//                 if (!response.iserror) break;
//
//                 if (!(repeatable = response.isrepeatable))
//                 {
//                     Debug.WriteLine("Sync Set Externals {Error}", response.error);
//                     break;
//                 }
//
//                 if (++retries >= maxRetries) break;
//             }
//             catch (Exception ex)
//             {
//                 Debug.WriteLine($"An error occurred: {ex.Message}");
//                 break;
//             }
//         } while (repeatable);
//     }
//
//     public async Task<List<ComponentBase>> SyncGetTaskInfo(string id)
//     {
//         var request = new syncGetTaskInfo
//         {
//             company = _companyId,
//             id = id
//         };
//         var response =
//             await _httpClientExtension.SendRequestAsync<syncGetTaskInfo, syncGetTaskInfoResult>(
//                 ApiUrl + "/SyncGetTaskInfo", request);
//         return new List<ComponentBase>();
//     }
//
//     public async Task<List<ComponentBase>> SyncGetChanges()
//     {
//         Debug.WriteLine("SyncGetChanges");
//         List<ComponentBase> changes = new List<ComponentBase>();
//         var moredata = false;
//         do
//         {
//             var request = new syncGetChanges
//             {
//                 company = _companyId,
//                 lasttranId = (await SyncGetCurrentTranid())!
//             };
//             var response =
//                 await _httpClientExtension.SendRequestAsync<syncGetChanges, syncGetChangesResponse>(
//                     ApiUrl + "/SyncGetChanges", request);
//
//             if (response.iserror)
//             {
//                 if (response.isrepeatable) continue;
//                 Console.WriteLine((string?)response.error);
//                 break;
//             }
//
//             foreach (var tran in response.trans)
//             {
//                 changes.AddRange(tran.changes);
//                 _lastTranId = tran.tranid;
//             }
//
//             _lastTranId = response.lasttranid ?? _lastTranId;
//             moredata = response.moredata;
//         } while (moredata);
//
//         return changes;
//     }
//
//     public async Task<List<ComponentBase>> SyncGetInitialChanges()
//     {
//         Debug.WriteLine("SyncGetInitialChanges");
//         var moredata = false;
//         string? lastComponentId = null;
//         List<ComponentBase> initialChanges = new();
//         do
//         {
//             var request = new syncGetInitialChanges
//             {
//                 maxtranid = await SyncGetCurrentTranid(),
//                 company = _companyId,
//                 withexternalid = _withExternalId,
//                 lastcomponentid = lastComponentId
//             };
//             var response =
//                 await _httpClientExtension.SendRequestAsync<syncGetInitialChanges, syncGetInitialChangesResponse>(
//                     ApiUrl + "/SyncGetInitialChanges", request);
//             if (response.iserror)
//             {
//                 if (response.isrepeatable) continue;
//                 Console.WriteLine((string?)response.error);
//                 break;
//             }
//
//             initialChanges.AddRange(response.components);
//             moredata = response.moredata;
//             lastComponentId = response.lastcomponentid;
//         } while (moredata);
//
//         return initialChanges;
//     }
//
//     public async Task<ComponentBase> SyncGetSimpleComponents(string id)
//     {
//         var request = new syncGetSimpleComponents
//         {
//             company = _companyId,
//             ids = new List<apiComponentIdentification>
//             {
//                 new()
//                 {
//                     id = id,
//                     eid = null!
//                 }
//             }
//         };
//         var response =
//             await _httpClientExtension.SendRequestAsync<syncGetSimpleComponents, syncGetSimpleComponentsResult>(
//                 ApiUrl + "/SyncGetSimpleComponents", request);
//         if (response.iserror) Console.WriteLine((string?)response.error);
//         return response.components.FirstOrDefault()!;
//         //await HandleComponents(response.components);
//     }
//
//     public async Task<string?> SyncGetCurrentTranid()
//     {
//         Debug.WriteLine("SyncGetCurrentTranid");
//         using (var scope = _serviceProvider.CreateScope())
//         {
//             var transactionService = scope.ServiceProvider.GetService<ITransactionService>();
//             var tranId = await transactionService!.GetLatestTransactionId();
//             if (tranId != string.Empty) return tranId;
//         }
//
//         bool loop;
//         do
//         {
//             loop = false;
//             var request = new syncGetCurrentTranid
//             {
//                 company = _companyId
//             };
//             var response =
//                 await _httpClientExtension.SendRequestAsync<syncGetCurrentTranid, syncGetCurrentTranidResponse>(
//                     ApiUrl + "/SyncGetCurrentTranid", request);
//             if (response.iserror)
//             {
//                 if (response.isrepeatable) loop = true;
//                 else Console.WriteLine($"{response.error}");
//             }
//             else
//             {
//                 using var scope = _serviceProvider.CreateScope();
//                 return response.tranid;
//             }
//         } while (loop);
//
//         return null;
//     }
//
//     public async Task<List<syncResultChange>> SyncSetChanges(List<ComponentBase> changes)
//     {
//         List<syncResultChange> results = new();
//         bool loop;
//         do
//         {
//             loop = false;
//
//             var request = new syncSetChanges
//             {
//                 company = _companyId,
//                 usercompany = _userCompanyId,
//                 changes = changes
//             };
//
//             var response =
//                 await _httpClientExtension.SendRequestAsync<syncSetChanges, syncSetChangesResult>(
//                     ApiUrl + "/SyncSetChanges", request);
//
//             if (response.iserror)
//             {
//                 if (loop = response.isrepeatable)
//                     continue;
//                 Console.WriteLine((string?)response.error);
//             }
//
//             results.AddRange(response.results);
//         } while (loop);
//
//         return results;
//     }
// }