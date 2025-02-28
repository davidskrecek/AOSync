using Microsoft.Extensions.DependencyInjection;

namespace AOSync.COMMON.Installers;

public interface IInstaller
{
    public void Install(IServiceCollection serviceCollection);
}