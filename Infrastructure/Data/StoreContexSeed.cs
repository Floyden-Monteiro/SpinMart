// using System.Text.Json;
// using Core.Entities;
// using Microsoft.Extensions.Logging;
// using Microsoft.EntityFrameworkCore;


// namespace Infrastructure.Data;

// public class StoreContextSeed
// {
//     public static async Task SeedAsync(StoreContext context, ILogger<StoreContextSeed> logger)
//     {
//         try
//         {
//             await SeedDataAsync<ProductBrand>(context.ProductBrands, "../Infrastructure/Data/SeedData/brands.json", "ProductBrands", context, logger);
//             await SeedDataAsync<ProductType>(context.ProductTypes, "../Infrastructure/Data/SeedData/types.json", "ProductTypes", context, logger);
//             await SeedDataAsync<Product>(context.Products, "../Infrastructure/Data/SeedData/products.json", "Products", context, logger);
//             await SeedDataAsync<User>(context.Users, "../Infrastructure/Data/SeedData/users.json", "Users", context, logger);
//             await SeedDataAsync<Customer>(context.Customers, "../Infrastructure/Data/SeedData/customers.json", "Customers", context, logger);

//             if (context.ChangeTracker.HasChanges())
//             {
//                 logger.LogInformation("Saving changes to the database...");
//                 await context.SaveChangesAsync();
//                 logger.LogInformation("Database seeding completed successfully.");
//             }
//             else
//             {
//                 logger.LogInformation("No changes detected in the ChangeTracker.");
//             }
//         }
//         catch (Exception ex)
//         {
//             logger.LogError(ex, "An error occurred during seeding.");
//         }
//     }

//     private static async Task SeedDataAsync<T>(
//         DbSet<T> dbSet, string filePath, string entityName, StoreContext context, ILogger logger) where T : class
//     {
//         if (!dbSet.Any())
//         {
//             logger.LogInformation($"Seeding {entityName}...");
//             var jsonData = await File.ReadAllTextAsync(filePath);
//             var data = JsonSerializer.Deserialize<List<T>>(jsonData);

//             if (data != null && data.Count > 0)
//             {
//                 dbSet.AddRange(data);
//                 logger.LogInformation($"{entityName} seeded successfully.");
//             }
//             else
//             {
//                 logger.LogWarning($"No {entityName} data found to seed.");
//             }
//         }
//         else
//         {
//             logger.LogInformation($"{entityName} already exists in the database.");
//         }
//     }
// }
