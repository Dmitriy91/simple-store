using Store.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace Store.Data
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

            context.Database.ExecuteSqlCommand(@"
                IF NOT EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME='Log' AND xtype='U')
                    CREATE TABLE [dbo].[Log] (
                        [Id] [int] IDENTITY (1, 1) NOT NULL,
                        [Date] [datetime] NOT NULL,
                        [Thread] [varchar] (255) NOT NULL,
                        [Level] [varchar] (50) NOT NULL,
                        [Logger] [varchar] (255) NOT NULL,
                        [Message] [varchar] (4000) NOT NULL,
                        [Exception] [varchar] (2000) NULL)");
        }
    } 
}
