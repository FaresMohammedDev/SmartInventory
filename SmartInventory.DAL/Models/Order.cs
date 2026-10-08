using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventory.DAL.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int? userId { get; set; }
        public ApplicationUser? User { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime DateTime { get; set; } = DateTime.UtcNow;

        public ICollection<Item> Items { get; set; } = new HashSet<Item>();
    }
}
