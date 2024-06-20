using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace AOSync.Model
{
    internal class SyncGetChanges_Response
    {
        public string lasttranid { get; set; }
        public bool moredata { get; set; }
        public List<SyncGetTransaction> trans { get; set; }

        public bool iserror { get; set; }

        public string error { get; set; }

        public bool isrepeatable { get; set; }

        public string ToString()
        {
            string self = $"{this.lasttranid}, {this.moredata}, {this.iserror}, {this.error}, {this.isrepeatable}";
            return self;
        }

    }
}
