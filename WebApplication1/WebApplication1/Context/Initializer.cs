using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using WebApplication1.Models;

namespace WebApplication1.Context
{
    public static class Initializer
    {

        public class UserModel
        {            
            public string Id { get; set; }
            public string UserName { get; set; }
            public string UserRole { get; set; }
        }

        public static void InitializeReqests(this ServiceContext context)
        {
            if (context.Requests.Any()) return;

            Request request1 = new Request()
            {
                ID = 1,
                RequestIn = DateTime.Now,
                FullName = "Первый заказчик",
                Contact = "f@mail.ru",
                RequestText = "Чем кормить хомячка?",
                PerformingInfo = "",
                Status = "received"
            };

            Request request2 = new Request()
            {
                ID = 2,
                RequestIn = DateTime.Now,
                FullName = "Второй заказчик",
                Contact = "s@mail.ru",
                RequestText = "Как достать звезду?",
                PerformingInfo = "",
                Status = "received"
            };

            Request request3 = new Request()
            {
                ID = 3,
                RequestIn = DateTime.Now,
                FullName = "Третий заказчик",
                Contact = "t@mail.ru",
                RequestText = "Как подняться на К2?",
                PerformingInfo = "",
                Status = "received"
            };


            using (var transaction = context.Database.BeginTransaction())
            {
                context.Requests.Add(request1);
                context.Requests.Add(request2);
                context.Requests.Add(request3);
                context.Database.ExecuteSqlInterpolated($"SET IDENTITY_INSERT [dbo].[Requests] ON");
                context.SaveChanges();
                context.Database.ExecuteSqlInterpolated($"SET IDENTITY_INSERT [dbo].[Requests] OFF");
                transaction.Commit();
            }
   
        }

        public static async Task InitializeUserAsync(this ServiceProvider provider)
        {
            var context = provider.GetRequiredService<ServiceContext>();

            var userManager = provider.GetRequiredService<UserManager<AppUser>>();
            var roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();

            var allRoles = roleManager.Roles.ToList();
            if (allRoles.Count == 0)
            {

                // admin, work, blog, progect, service, contact, mainpage, myrole, users
                await roleManager.CreateAsync(new IdentityRole { Name = "admin" });
                await roleManager.CreateAsync(new IdentityRole { Name = "work" });
                await roleManager.CreateAsync(new IdentityRole { Name = "blog" });
                await roleManager.CreateAsync(new IdentityRole { Name = "progect" });
                await roleManager.CreateAsync(new IdentityRole { Name = "service" });
                await roleManager.CreateAsync(new IdentityRole { Name = "contact" });
                await roleManager.CreateAsync(new IdentityRole { Name = "mainpage" });
                await roleManager.CreateAsync(new IdentityRole { Name = "myrole" });
                await roleManager.CreateAsync(new IdentityRole { Name = "users" });
            }

            var admins = from user in context.Users
                         join userRole in context.UserRoles on user.Id equals userRole.UserId
                         join roles in context.Roles on userRole.RoleId equals roles.Id
                         where roles.Name == "admin"
                         select new UserModel
                         {
                             Id = user.Id,
                             UserName = user.UserName,
                             UserRole = roles.Name,
                         };

            var a = admins.ToList();
            if (a.Any()) return;

            AppUser admin = new AppUser()
            {
                UserName = "Admin"
            };

            string defaultPassword = "123qwe";

            var result = await userManager.CreateAsync(admin, defaultPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "admin");
            }
        }
    }
}
