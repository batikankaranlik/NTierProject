using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ENTITIES.Models
{
    public class UserProfile:BaseEntity
    {
        [Required(ErrorMessage = "Girilmesi Zorunludur")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Girilmesi Zorunludur")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Girilmesi Zorunludur")]
        public string Address { get; set; }

        //Relational Properties
        public virtual AppUser AppUser { get; set; }
    }
}
