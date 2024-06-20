using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOSync.Model
{
    internal class SyncChangeSimple
    {
        public string type { get; set; }
        public bool simple { get; set; }
        public string def { get; set; }
        public string eid { get; set; }
        public SyncParent parent { get; set; }
        public bool archived { get; set; }
        public ApiAttributeValue attrs { get; set; }
        public string creator { get; set; }
        public DateTime created { get; set; }
        public ApiValue fields { get; set; }
    }
}
