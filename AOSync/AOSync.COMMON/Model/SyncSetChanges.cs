using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using AOSync.COMMON.Converters;

namespace AOSync.COMMON.Model;

public class syncSetChanges
{
    [StringLength(14, MinimumLength = 14)] public string company { get; set; } = null!;

    [StringLength(14, MinimumLength = 14)] public string usercompany { get; set; } = null!;

    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime ts { get; set; }

    public string nonotifications { get; set; } = null!;

    [JsonConverter(typeof(ChangesToDefClassConverter))]
    public List<ComponentBase> changes { get; set; } = new();

    public override string ToString()
    {
        return
            $"company: {company}, usercompany: {usercompany}, ts: {ts}, nonotifications: {nonotifications}, changes: [{string.Join(", ", changes)}]";
    }
}