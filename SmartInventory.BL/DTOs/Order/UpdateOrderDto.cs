using SmartInventory.BL.DTOs.Item;
using SmartInventory.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventory.BL.DTOs.Order
{
    public record UpdateOrderDto
    {
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
        public List<UpdateItemDto> items { get; set; } = new List<UpdateItemDto>();
    }
}
