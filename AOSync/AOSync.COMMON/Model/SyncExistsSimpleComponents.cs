using System.ComponentModel.DataAnnotations;

namespace AOSync.COMMON.Model;

public class syncExistsSimpleComponents
{
    [StringLength(14, MinimumLength = 14)] public string company { get; set; } = string.Empty;

    public List<apiComponentIdentification> ids { get; set; } = new();
}