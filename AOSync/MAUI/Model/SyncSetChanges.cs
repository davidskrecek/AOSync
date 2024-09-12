using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MAUI.Model
{
    internal class syncSetChanges
    {
        [StringLength(14, MinimumLength = 14)]
        public string company { get; set; } = null!;
        [StringLength(14, MinimumLength = 14)]
        public string usercompany { get; set; } = null!;
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime ts { get; set; }
        public string nonotifications { get; set; } = null!;
        public List<syncChange> changes { get; set; } = new List<syncChange>();
    }
}
