namespace AOSync.COMMON.Model;

public class syncExistsSimpleComponentsResult
{
    public List<apiComponentExists> data { get; set; } = new();
    public bool iserror { get; set; } = false;
    public string error { get; set; } = null!;
    public bool isrepeatable { get; set; } = false;
}