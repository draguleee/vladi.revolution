using vladi.revolution.Data.Enums;
using vladi.revolution.Models;

namespace vladi.revolution.Data
{
    public class AppDbInitializer
    {
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
    }
}
