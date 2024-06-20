using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOSync.Model
{
    internal class SyncGetCurrentTranid_Request
    {
        public string company { get; set; }

        public string ToString()
        {
            return company;
        }
    }
}
