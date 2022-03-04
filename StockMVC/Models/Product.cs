using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StockMVC.Models
{
    public class Product
    {
        [Key]
        [StringLength(6)]
        public string Code { get; set; }
    }
}
