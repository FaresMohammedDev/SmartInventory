using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventory.BL.DTOs.Item
{
    public record UpdateItemDto
    {
        public int productId { get; set; }
        public int orderId { get; set; }
        public int Quantity { get; set; }
    }
}
