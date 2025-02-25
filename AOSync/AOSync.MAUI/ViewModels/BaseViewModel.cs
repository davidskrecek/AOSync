using CommunityToolkit.Mvvm.ComponentModel;

namespace AOSync.MAUI.ViewModels;

public abstract class BaseViewModel : ObservableObject
{
    protected IServiceProvider? ServiceProvider { get; set; }
}