using Microsoft.AspNetCore.Identity;
using vladi.revolution.Data.Enums;
using vladi.revolution.Data.Static;
using vladi.revolution.Models;

namespace vladi.revolution.Data
{
    public class AppDbInitializer
    {
        /*
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
                context.Database.EnsureCreated();
                if (!context.Players.Any())
                {
                    context.Players.AddRange(new List<Player>()
                    {
                        new Player()
                        {
                            ProfilePictureUrl = "https://cdn-icons-png.freepik.com/512/166/166344.png",
                            FullName = "Fabian Gandi",
                            BirthDate = new DateOnly(2000, 9, 25),
                            Position = "Atacant",
                            Biography = "Biografie Fabian. Va urma."
                        },
                        new Player()
                        {
                            ProfilePictureUrl = "https://cdn-icons-png.freepik.com/512/166/166344.png",
                            FullName = "Dorin Gandi",
                            BirthDate = new DateOnly(2000, 9, 25),
                            Position = "Portar",
                            Biography = "Biografie Dorin. Va urma."
                        },
                        new Player()
                        {
                            ProfilePictureUrl = "https://cdn-icons-png.freepik.com/512/166/166344.png",
                            FullName = "Flavius Dărău (Sisa)",
                            BirthDate = new DateOnly(2000, 9, 25),
                            Position = "Mijlocaș",
                            Biography = "Biografie Sisa. Va urma."
                        },
                        new Player()
                        {
                            ProfilePictureUrl = "https://cdn-icons-png.freepik.com/512/166/166344.png",
                            FullName = "Andrei Țica",
                            BirthDate = new DateOnly(2000, 9, 25),
                            Position = "Fundaș",
                            Biography = "Biografie Andrei. Va urma."
                        }
                    });
                    context.SaveChanges();
                }
            }
        }

        */

        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                // Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

                // Admin & Users 
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                string adminUserEmail = "vladirevolution00@gmail.com";
                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                if (adminUser == null)
                {
                    var newAdminUser = new AppUser()
                    {
                        FullName = "Administrator",
                        UserName = "administrator",
                        Email = adminUserEmail,
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(newAdminUser, "VladiRevolution@2024!");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                }

                string userEmail = "aandreid14@gmail.com";
                var user = await userManager.FindByEmailAsync(userEmail);
                if (user == null)
                {
                    var newUser = new AppUser()
                    {
                        FullName = "Andreea Dragu",
                        UserName = "draguleee",
                        Email = userEmail,
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(newUser, "Skatemaster16!");
                    await userManager.AddToRoleAsync(newUser, UserRoles.User);
                }
            }
        }
    }
}
