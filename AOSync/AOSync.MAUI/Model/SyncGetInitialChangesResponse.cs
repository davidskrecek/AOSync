using Newtonsoft.Json;

namespace AOSync.MAUI.Model;

internal class syncGetInitialChangesResponse
{
    public string lastcomponentid { get; set; } = null!;
    public bool moredata { get; set; } = false;

    [JsonConverter(typeof(ComponentBaseConverter))]
    public List<ComponentBase> components { get; set; } = new();

    public bool iserror { get; set; } = false;
    public string error { get; set; } = null!;
    public bool isrepeatable { get; set; } = false;

    public override string ToString()
    {
        var componentsString = components != null ? string.Join(", ", components.Select(c => c.ToString())) : "null";
        return $"Last Component ID: {lastcomponentid}, More Data: {moredata}, Components: [{componentsString}], " +
               $"Is Error: {iserror}, Error: {error}, Is Repeatable: {isrepeatable}";
    }
}