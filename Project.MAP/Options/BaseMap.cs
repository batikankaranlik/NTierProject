using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.MAP.Options
{
   public abstract class BaseMap<T>:EntityTypeConfiguration<T> where T:BaseEntity
    {
        //BaseMap tabloları düzenlememizi sağlıyor ve sadece BaseEntityden çektiği için başka bir tür class girmemizi engeliyor
        public BaseMap()
        {
            //Burada Tablolaradki ortak stün adlarını değiştirmek için kullandığımız kod
            Property(x => x.CreatedDate).HasColumnName("Veri Yaratma Tarihi").HasColumnType("datetime2");
            //devamnı yazabilirsin
            Property(x => x.Status).HasColumnName("Veri Durumu");

        }
    }
}
