using AOSync.COMMON.Models;
using AOSync.DAL.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Pomelo.EntityFrameworkCore.MySql.Query.ExpressionVisitors.Internal;

namespace AOSync.BL.ProcessingModules;

public class SetExternalsProcessor : IProcessorBase
{
    private static IServiceProvider _serviceProvider;
    private static IConfiguration _configuration;
    
    public void Initialize(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _configuration = configuration;
        
    }

    public async Task HandleResult(SyncSetExternalsResult result)
    {
        if (result.Iserror == true)
        {
            Console.WriteLine("[ERROR] couldnt synch externals");
        }
    }
}