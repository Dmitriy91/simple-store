using Assignment.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace Assignment.Data
{
    public class AppDbContextInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            ApplicationUser admin = new ApplicationUser
            {
                Email = "admin@gmail.com",
                UserName = "admin@gmail.com"
            };
            IdentityResult umCreateResult = userManager.Create(admin, "Admin123!");

            if (umCreateResult.Succeeded)
            {
                RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                IdentityResult rmCreateResult = roleManager.Create(new IdentityRole("Admin"));

                if (rmCreateResult.Succeeded)
                {
                    IdentityResult umAddToRoleResult = userManager.AddToRole(admin.Id, "Admin");

                    if(umAddToRoleResult.Succeeded)
                        context.SaveChanges();
                }
            }
        }
    } 
}
