using Newtonsoft.Json;

namespace AOSync.MAUI.Model;

internal class syncGetSimpleComponentsResult
{
    [JsonConverter(typeof(ComponentBaseConverter))]
    public List<ComponentBase> components { get; set; } = new();

    public bool iserror { get; set; } = false;
    public string error { get; set; } = null!;
    public bool isrepeatable { get; set; } = false;
}