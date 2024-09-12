using AOSync.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AOSync.Model
{
    public class syncGetTransaction
    {
        public string tranid { get; set; } = null!;
        public string ts { get; set; } = null!;
        public string usercompany { get; set; } = null!;
        [JsonConverter(typeof(ComponentBaseConverter))]
        public List<ComponentBase> changes { get; set; } = new List<ComponentBase>();

        public override string ToString()
        {
            var changesString = string.Join(", ", changes.Select(c => c.ToString()));
            return $"TranId: {tranid}, TS: {ts}, UserCompany: {usercompany}, Changes: [{changesString}]";
        }
    }
}
