using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOSync.Model
{
    internal class SyncGetTransaction
    {
        public string tranid { get; set; }
        public DateTime ts { get; set; }
        public string usercompany { get; set; }
        public List<SyncChangeBase> changes { get; set; }
    }
}
