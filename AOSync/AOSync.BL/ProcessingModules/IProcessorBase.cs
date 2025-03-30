using Microsoft.Extensions.Configuration;

namespace AOSync.BL.ProcessingModules;

public interface IProcessorBase
{
    public void Initialize(IServiceProvider serviceProvider, IConfiguration configuration);
}