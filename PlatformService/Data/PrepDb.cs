using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProd)
        {
            using(var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProd);
            }
        }

        private static void SeedData(AppDbContext context, bool isProd)
        {
            if(isProd)
            {
                Console.WriteLine("--> Attemping to apply migrations...");
                try
                {
                    context.Database.Migrate();
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"--> Could not run migrations: {ex.Message}");
                }
            }

            if(!context.Platforms.Any())
            {
                Console.WriteLine("--> seeding data...");     
                
                context.Platforms.AddRange(
                    new Platform{Name = "dot net", Publisher = "Microsoft", Cost = "free"},
                    new Platform{Name = "SQL SERVER", Publisher = "Microsoft", Cost = "free"},
                    new Platform{Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation", Cost = "free"}
                );
                
            context.SaveChanges();
            }
            else
            {
                Console.WriteLine("--> we already have data");
            }
        }
    }
}