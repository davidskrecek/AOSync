namespace AOSync.COMMON.Model;

public class syncResultChange
{
    public string type { get; set; } = null!;
    public string eid { get; set; } = null!;
    public string addedid { get; set; } = null!;
    public string linkedid { get; set; } = null!;

    public override string ToString()
    {
        return $"type: {type}, eid: {eid}, addedid: {addedid}, linkedid: {linkedid}";
    }
}