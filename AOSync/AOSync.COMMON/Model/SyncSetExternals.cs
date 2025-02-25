using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace AOSync.COMMON.Model;

public class syncSetExternals
{
    [StringLength(14, MinimumLength = 14)] public string company { get; set; } = null!;

    public List<syncExternal> externals { get; set; } = new();

    public string ToString()
    {
        string text = String.Empty;
        foreach (var external in externals)
        {
            text += external.ToString() + ", ";
        }

        return text;
    }
}