using Microsoft.EntityFrameworkCore;
using StockMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockMVC.Data
{
    public class StockContext:DbContext
    {
        public StockContext(DbContextOptions options):base(options)
        {

        }
        public virtual DbSet<Unit> Units { get; set; }
        public virtual DbSet<Product> Products { get; set; }
    }
}
