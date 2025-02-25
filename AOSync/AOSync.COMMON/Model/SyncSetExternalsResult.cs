namespace AOSync.COMMON.Model;

public class syncSetExternalsResult
{
    public bool iserror { get; set; } = false;
    public string error { get; set; } = null!;
    public bool isrepeatable { get; set; } = false;
}