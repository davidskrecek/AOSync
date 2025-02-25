namespace AOSync.COMMON.Model;

public class syncExternal
{
    public string id { get; set; } = string.Empty!;
    public string eid { get; set; } = string.Empty!;

    public string ToString()
    {
        return $"id: {id}; eid: {eid}";
    }
}