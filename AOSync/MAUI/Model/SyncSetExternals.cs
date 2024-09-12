using System.ComponentModel.DataAnnotations;

namespace MAUI.Model
{
    internal class syncSetExternals
    {
        [StringLength(14, MinimumLength = 14)]
        public string company { get; set; } = null!;
        public List<syncExternal> externals { get; set; } = new List<syncExternal>();
    }
}
