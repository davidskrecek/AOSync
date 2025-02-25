using System.ComponentModel.DataAnnotations;

namespace AOSync.COMMON.Model;

public class syncGetTimesheetInfo
{
    [StringLength(14, MinimumLength = 14)] public string company { get; set; } = null!;

    [StringLength(14, MinimumLength = 14)] public string id { get; set; } = null!;
}