using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ENTITIES.Models
{
    public class Category:BaseEntity
    {
        public Category()
        {
            Products = new List<Product>();
        }
        [Required(ErrorMessage = "Girilmesi Zorunludur")]
        public String CategoryName { get; set; }
        [Required(ErrorMessage = "Girilmesi Zorunludur")]
        public string Description { get; set; }

        //Relational Properties
        public virtual List<Product> Products { get; set; }
    }
}
