using SmartInventory.DAL.Data;
using SmartInventory.DAL.Models;
using SmartInventory.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventory.DAL.Repositories.Implementations
{
    public class ItemRepo : GenericRepo<Item>, IItemRepo
    {
        private readonly ApplicationDbContext _context;
        public ItemRepo(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
