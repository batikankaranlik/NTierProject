using Project.ENTITIES.Models;
using Project.DAL.ContextClasses;

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.COMMON.Tools;

namespace Project.DAL.StrategyPattern
{
    public class MyInit:CreateDatabaseIfNotExists<MyContext>
    {
        protected override void Seed(MyContext context)
        {
            #region admin
            AppUser au = new AppUser();
            au.UserName = "BK";
            au.Password = DantexCrypt.Crypt("123");
            au.ConfirmPassword = DantexCrypt.Crypt("123");
            au.Email = "relord8@gmail.com";
            au.Active = true;
            au.Role = ENTITIES.Enums.UserRole.Admin;
            
            context.AppUsers.Add(au);
            context.SaveChanges();
            #endregion

            AppUser ap = new AppUser();
            ap.UserName = "dgk";
            ap.Password = DantexCrypt.Crypt("123");
            ap.ConfirmPassword = DantexCrypt.Crypt("123");           
            ap.Email = "xxx";
            ap.Active = true;
            context.AppUsers.Add(ap);
            context.SaveChanges();
            

            UserProfile up = new UserProfile();
            up.ID = 2;
            up.FirstName = "doğukan";
            up.LastName = "demirtaş";
            up.Address = "xxx";
            context.UserProfiles.Add(up);
            context.SaveChanges();

            Category c = new Category();
            c.CategoryName = "fitre";
            c.Description = "güzel";

            Product p = new Product();
            p.ProductName = "kahve";
            p.UnitPrice = 30;
            p.UnitsInStock = 50;
            //p.ImagePath=
            c.Products.Add(p);

            context.Categories.Add(c);
            context.SaveChanges();

        

        }





    }
}
