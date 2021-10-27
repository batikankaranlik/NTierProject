using Project.ENTITIES.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ENTITIES.Models
{
   public class Order:BaseEntity
    {
        [Required(ErrorMessage = "bu alan boş bırakılamaz")]
        public string ShippedAddress { get; set; }
        public decimal TotalPrice { get; set; }
        public int? AppUserID { get; set; }

        public String UserName { get; set; }
        [Required(ErrorMessage ="bu alan boş bırakılamaz")]
        public string Email { get; set; }

        public DeliveryStatus Delivery { get; set; }
        public Order()
        {
            Delivery = DeliveryStatus.Hazırlanıyor;
        }

        //Relational Properties
        public virtual AppUser AppUser { get; set; }
        public virtual List<OrderDetail> OrderDetails { get; set; }
    }
}
