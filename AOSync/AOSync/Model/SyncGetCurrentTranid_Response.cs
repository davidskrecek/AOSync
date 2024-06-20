using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOSync.Model
{
    internal class SyncGetCurrentTranid_Response
    {
        public string tranid { get; set; }
        public bool iserror { get; set; }
        public string error { get; set; }
        public bool isrepeatable { get; set; }

        public string ToString()
        {
            return $"{tranid}, {iserror}, {error}, {isrepeatable}";
        }
    }
}
