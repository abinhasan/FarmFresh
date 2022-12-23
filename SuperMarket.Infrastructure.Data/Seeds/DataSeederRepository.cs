using Microsoft.AspNetCore.Identity;
using SuperMarket.Domain.Entities.Contexts;
using SuperMarket.Domain.Entities.Entities;
using SuperMarket.Domain.Interfaces;

namespace SuperMarket.Infrastructure.Data.Seeds
{
    public class DataSeederRepository : IDataSeederRepository
    {
        private readonly ApplicationDbContext dbContext;

        private readonly UserManager<ApplicationUser> userManager;

        public DataSeederRepository(ApplicationDbContext dbContext,
            UserManager<ApplicationUser> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        public async Task SeedData()
        {
            var email = "basicuser@gmail.com";

            var user = await userManager.FindByEmailAsync(email.Trim().Normalize());

            if (user != null)
            {
                if (!dbContext.Categories.Any())
                {
                    var categories = new List<Category>()
                    {
                        new Category()
                        {
                            Name = "Fruits & Vegetables",
                            IsActive = true
                        },
                        new Category()
                        {
                            Name = "Baby Food",
                            IsActive = true
                        }
                    };

                    await dbContext.Categories.AddRangeAsync(categories);
                    await dbContext.SaveChangesAsync();
                }

                if (!dbContext.Products.Any() & dbContext.Categories.Any())
                {
                    var categories = dbContext.Categories.ToList();

                    var products = new List<Product>();

                    foreach (var item in categories)
                    {
                        for (int i = 0; i < 100; i++)
                        {
                            products.Add(new Product()
                            {
                                Title = "Title test " + (i + 1),
                                Code = "Code test " + (i + 1),
                                Sku = "Sku test " + (i + 1),
                                Price = 40 + i,
                                Description = (i + 1) + "Description We believe time is valuable to our fellow residents, and that they should not have to waste hours in traffic, brave bad weather and wait in line just to buy basic necessities like eggs! This is why Chaldal delivers everything you need right at your door-step and at no additional cost" + (i + 1),
                                IsActive = true,
                                CategoryId = item.Id
                            });
                        }
                    }

                    await dbContext.Products.AddRangeAsync(products);
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
