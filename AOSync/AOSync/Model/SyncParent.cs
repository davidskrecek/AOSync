using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AOSync.Model
{
    public class syncParent
    {
        [StringLength(14, MinimumLength = 3)]
        public string id { get; set; } = null!;
        [StringLength(100, MinimumLength = 1)]
        public string field { get; set; } = null!;

        public override string ToString()
        {
            return $"ID: {id}, Field: {field}";
        }
    }
}
