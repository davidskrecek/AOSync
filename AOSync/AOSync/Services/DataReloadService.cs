using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace AOSync.Services;
public class DataReloadService
{
    private readonly List<Func<Task>> _reloadActions = new();

    public void RegisterReloadAction(Func<Task> reloadAction)
    {
        _reloadActions.Add(reloadAction);
    }

    public async void NotifyDataChanged()
    {
        foreach (var reloadAction in _reloadActions)
        {
            await reloadAction();
        }
    }
}