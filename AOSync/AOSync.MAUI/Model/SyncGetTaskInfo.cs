using System.ComponentModel.DataAnnotations;

namespace AOSync.MAUI.Model;

internal class syncGetTaskInfo
{
    [StringLength(14, MinimumLength = 14)] public string company { get; set; } = null!;

    [StringLength(14, MinimumLength = 14)] public string id { get; set; } = null!;
}