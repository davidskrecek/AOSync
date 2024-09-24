using System.ComponentModel.DataAnnotations;

namespace AOSync.MAUI.Model;

internal class syncGetSimpleComponents
{
    [StringLength(14, MinimumLength = 14)] public string company { get; set; } = null!;

    public List<apiComponentIdentification> ids { get; set; } = new();
    public bool simpleResult { get; set; } = false;
}