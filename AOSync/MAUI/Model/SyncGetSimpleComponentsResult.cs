using AOSync.Converters;
using Newtonsoft.Json;

namespace MAUI.Model
{
    internal class syncGetSimpleComponentsResult
    {
        [JsonConverter(typeof(ComponentBaseConverter))]
        public List<ComponentBase> components { get; set; } = new List<ComponentBase>();
        public bool iserror { get; set; } = false;
        public string error { get; set; } = null!;
        public bool isrepeatable { get; set; } = false;
    }
}
