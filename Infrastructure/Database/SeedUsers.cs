using Domain.Aggregate;
using Domain.Value_Object;
using Infrastructure.Database.DBModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Database
{
    public static class SeedUsers
    {
        public static async Task OnStart(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.Database.Migrate();
                if (!dbContext.Users.Any())
                {
                    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserModel>>();
                    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<RoleModel>>();

                    if (!await roleManager.RoleExistsAsync("Admin"))
                    {
                        await roleManager.CreateAsync(new RoleModel("Admin"));
                    }
                    if (!await roleManager.RoleExistsAsync("User"))
                        await roleManager.CreateAsync(new RoleModel("User"));
                    var adminEmail = "test@example.com";
                    var adminUser = await userManager.FindByEmailAsync(adminEmail);

                    if (adminUser == null)
                    {
                        var admin = new UserModel
                        {
                            UserName = "Admin",
                            Email = adminEmail
                        };
                        var result = await userManager.CreateAsync(admin, "BardzoSilneHaslo123!");

                        if (result.Succeeded)
                        {
                            await userManager.AddToRoleAsync(admin, "Admin");
                            await userManager.AddToRoleAsync(admin, "User");
                            var adminDetails = UserDetails.Create(admin.Id, new Fullname("Admin", "Admin"), new Username("Admin"), Email.Create(adminEmail));
                            await dbContext.UsersDetails.AddAsync(adminDetails);
                            await dbContext.SaveChangesAsync();
                        }
                    }
                }
            }
        }
    }
}
