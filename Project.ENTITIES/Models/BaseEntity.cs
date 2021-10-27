using Project.ENTITIES.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.ENTITIES.Models
{
    //kod tekrarı olmaması için her tabloda bulunacak bilgileri base sınıfı ile yazdık ve diğer sınıflara miras olarak vereceğiz.
    //Eğer miras olarak vermezsek o tabloda ID olmayacağı için oluşmaz.
    public abstract class BaseEntity
    {
        public int ID { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public DataStatus Status { get; set; } 
        public BaseEntity()
        {
            Status = DataStatus.Inserted;
            CreatedDate = DateTime.Now;
        }

        

    }
}
