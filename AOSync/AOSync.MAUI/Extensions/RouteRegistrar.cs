using System.Reflection;

namespace AOSync.MAUI.Extensions;

public static class Registrar
{
    /// <summary>
    /// Registers all Views ending with "View" as routes.
    /// </summary>
    public static void RegisterRoutes()
    {
        // Get all classes ending with "View" in the Views namespace
        var views = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsClass 
                        && t.Name.EndsWith("View") 
                        && t.Namespace?.Contains("Views") == true)
            .ToList();

        foreach (var view in views)
        {
            // Generate the route name by removing "View" suffix
            var routeName = view.Name.Replace("View", string.Empty); 
            Routing.RegisterRoute(routeName, view);
        }
    }
    
    /// <summary>
    /// Registers all ViewModel classes ending with "ViewModel" into services.
    /// </summary>
    public static void RegisterViewModels(IServiceCollection services)
    {
        // Get all classes ending with "ViewModel" in the Views namespace
        var viewModels = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsClass 
                        && t.Name.EndsWith("ViewModel") 
                        && t.Namespace?.Contains("ViewModels") == true
                        && !t.IsAbstract)
            .ToList();

        foreach (var viewModel in viewModels)
        {
            // Register the ViewModel class as transient in the DI container
            services.AddTransient(viewModel);
        }
    }
}
