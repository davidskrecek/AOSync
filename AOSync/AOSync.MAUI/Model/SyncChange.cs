using System.Text.Json.Serialization;

namespace AOSync.MAUI.Model;

internal class syncChange
{
    public string type { get; set; } = null!;
    public bool simple { get; set; } = false;
    public string def { get; set; } = null!;
    public string id { get; set; } = null!;
    public string eid { get; set; } = null!;
    public syncParent parent { get; set; } = new();
    public bool archived { get; set; } = false;
    public List<COMMON.Model.apiAttributeValue> attrs { get; set; } = null!;
    public string creator { get; set; } = string.Empty!;

    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime created { get; set; }

    public override string ToString()
    {
        return
            $"Type: {type}, Simple: {simple}, Def: {def}, ID: {id}, EID: {eid}, Parent: {{{parent.ToString() ?? "null"}}}, Archived: {archived}, Attrs: {attrs?.Count ?? 0}, Creator: {creator}, Created: {created}";
    }
}