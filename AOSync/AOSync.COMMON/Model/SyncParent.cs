using System.ComponentModel.DataAnnotations;

namespace AOSync.COMMON.Model;

public class syncParent
{
    [StringLength(14, MinimumLength = 3)] public string id { get; set; } = null!;

    [StringLength(100, MinimumLength = 1)] public string field { get; set; } = null!;
    public string def { get; set; } = "Attachment";
    public string? eid { get; set; }

    public override string ToString()
    {
        return $"ID: {id}, Field: {field}, Def: {def}, EID: {eid}";
    }
}