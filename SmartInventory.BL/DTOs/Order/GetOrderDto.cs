using SmartInventory.BL.DTOs.Item;
using SmartInventory.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventory.BL.DTOs.Order
{
    public class GetOrderDto
    {
        public int Id { get; set; }
        public string UserName { get; set; } = "Unkown";
        public decimal TotalPrice { get; set; }
        public DateTime DateTime { get; set; }
        public List<GetItemDto> Items { get; set; } = new List<GetItemDto>();
    }
}
