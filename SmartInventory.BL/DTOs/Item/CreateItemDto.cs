using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventory.BL.DTOs.Item
{
    public class CreateItemDto
    {
        public int Quantity { get; set; }
        public int productId { get; set; }
    }
}
