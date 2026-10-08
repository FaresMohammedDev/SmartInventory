using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventory.BL.Common
{
    public class PaginatedResult<T>
    {
        public List<T>? Data { get; set; }
        public int totalRecords { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
