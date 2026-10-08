using SmartInventory.BL.DTOs.Item;
using SmartInventory.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventory.BL.DTOs.Order
{
    public record CreateOrderDto
    {
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
        public List<CreateItemDto> Items { get; set; } = new List<CreateItemDto>();
    }
}
