using System.ComponentModel.DataAnnotations;

namespace MAUI.Model;

public class syncGetChanges
{
    [Required]
    [StringLength(14, MinimumLength = 14, ErrorMessage = "Company must be exactly 14 characters long.")]
    public string company { get; set; } = null!;

    public string lasttranId { get; set; } = string.Empty;

    [Range(0, 100, ErrorMessage = "Limit must be between 0 and 100.")]
    public int limit { get; set; } = 0;

    public bool simpleResult { get; set; } = false;

    public override string ToString()
    {
        string self = $"{this.company}, {this.lasttranId}, {this.limit}, {this.simpleResult}";
        return self;
    }
}