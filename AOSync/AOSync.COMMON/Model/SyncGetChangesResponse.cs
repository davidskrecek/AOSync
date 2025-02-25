namespace AOSync.COMMON.Model;

public class syncGetChangesResponse
{
    public string? lasttranid { get; set; }
    public bool moredata { get; set; }
    public List<syncGetTransaction> trans { get; set; } = new();

    public bool iserror { get; set; }

    public string error { get; set; } = null!;

    public bool isrepeatable { get; set; }

    public override string ToString()
    {
        var transString = string.Join(", ", trans.Select(t => t.ToString()));
        return
            $"LastTranId: {lasttranid}, MoreData: {moredata}, Trans: [{transString}], IsError: {iserror}, Error: {error}, IsRepeatable: {isrepeatable}";
    }
}