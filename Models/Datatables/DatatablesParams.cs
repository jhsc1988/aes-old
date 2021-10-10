using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aes.Models
{
    public class IDatatablesParams
    {
        public int Start { get; set; }
        public int Length { get; set; }
        public string SearchValue { get; set; }
        public string SortColumnName { get; set; }
        public string SortDirection { get; set; }
    }
}
