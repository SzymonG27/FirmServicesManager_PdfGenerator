using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PDFGenerator.Data;
using PDFGenerator.Models.AccountModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDFGenerator.Services
{
    public class SeedPpl
    {
        public static void EnsurePopulated(IApplicationBuilder app, UserManager<AppUser> userManager)
        {
            AppIdentityDbContext context = app.ApplicationServices
                .GetRequiredService<AppIdentityDbContext>();
            if (!context.Users.Any())
            {
                var user = new AppUser
                {
                    UserName = "Patrol",
                    NormalizedUserName = "Patrol".ToUpper(),
                    Email = "s.gajdzinski123@gmail.com",
                    NormalizedEmail = "s.gajdzinski123@gmail.com".ToUpper(),
                    EmailConfirmed = true,
                    FirstName = "Szymon",
                    SurName = "Gajdziński"
                };
                var res = userManager.CreateAsync(user, "Polaczki12").Result;
                if (res.Succeeded)
                {
                    //pog
                }
            }
            if (!context.Roles.Any())
            {
                IdentityRole role = new IdentityRole();
                role.Name = "RCON";
                role.NormalizedName = "RCON".ToUpper();
                context.Roles.Add(role);
                IdentityRole role2 = new IdentityRole();
                role2.Name = "Admin";
                role2.NormalizedName = "Admin".ToUpper();
                context.Roles.Add(role2);
                IdentityRole role3 = new IdentityRole();
                role3.Name = "Employer";
                role3.NormalizedName = "Employer".ToUpper();
                context.Roles.Add(role3);
                context.SaveChanges();
            }
        }
    }
}
