namespace AOSync.COMMON.Model;

public class syncGetCurrentTranidResponse
{
    public string tranid { get; set; }
    public bool iserror { get; set; }
    public string error { get; set; }
    public bool isrepeatable { get; set; }

    public override string ToString()
    {
        return $"{tranid}, {iserror}, {error}, {isrepeatable}";
    }
}