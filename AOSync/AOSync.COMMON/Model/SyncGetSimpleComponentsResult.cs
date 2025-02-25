using AOSync.COMMON.Converters;
using Newtonsoft.Json;

namespace AOSync.COMMON.Model;

public class syncGetSimpleComponentsResult
{
    [JsonConverter(typeof(ChangesToDefClassConverter))]
    public List<ComponentBase> components { get; set; } = new();

    public bool iserror { get; set; } = false;
    public string error { get; set; } = null!;
    public bool isrepeatable { get; set; } = false;
}