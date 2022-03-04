using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace StockMVC.Models
{
    public class Product
    {
        [Key]
        [StringLength(6)]
        public string Code { get; set; }
        [Required]
        [StringLength(75)]
        public String Name { get; set; }
        [Required]
        [StringLength(255)]
        public String Description { get; set; }
        [Required]
        [Column(TypeName = "smallmoney")]
        public decimal Cost { get; set; }
        [Required]
        [Column(TypeName = "smallmoney")]
        public decimal Price { get; set; }
        [Required]
        [ForeignKey("Units")]
        [Display(Name="Unit")]
        public int UnitId { get; set; }
        public virtual Unit Units { get; set; }

    }
}
