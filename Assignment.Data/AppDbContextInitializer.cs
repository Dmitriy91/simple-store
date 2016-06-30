using Assignment.Data.EntityConfigs;
using Assignment.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace Assignment.Data
{
    public class AppDbContextInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {

            //UserManager<ApplicationUser> manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            //ApplicationUser admin = new ApplicationUser
            //{
            //    Email = "admin@gmail.com",
            //    UserName = "admin@gmail.com"
            //};

            //IdentityResult result = manager.Create(admin, "Admin123!");

            //if (result.Succeeded)
            //{
            //    result = manager.AddToRole(admin.Id, "Admin");
            //}
            //context.SaveChanges();
        }
    } 
}
