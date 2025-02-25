using AOSync.COMMON.Converters;
using Newtonsoft.Json;

namespace AOSync.COMMON.Model;

public class syncGetTransaction
{
    public string tranid { get; set; } = null!;
    public string ts { get; set; } = null!;
    public string usercompany { get; set; } = null!;

    [JsonConverter(typeof(ChangesToDefClassConverter))]
    public List<ComponentBase> changes { get; set; } = new();

    public override string ToString()
    {
        var changesString = string.Join(", ", changes.Select(c => c.ToString()));
        return $"TranId: {tranid}, TS: {ts}, UserCompany: {usercompany}, Changes: [{changesString}]";
    }
}