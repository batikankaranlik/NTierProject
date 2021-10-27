using Project.ENTITIES.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.MAP.Options
{
    public class AppUserMap:BaseMap<AppUser>
    {
        public AppUserMap()
        {
            //Nu adla oluşsun
            ToTable("Kullanıcılar");

            //Bire bir ilişkinin tamamlanması için;
            HasOptional(x => x.Profile).WithRequired(x => x.AppUser);

            Ignore(x => x.ConfirmPassword);
            

        }
    }
}
