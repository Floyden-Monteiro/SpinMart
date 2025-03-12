using System.Text.Json;
using System.Text.Json.Serialization;
using Core.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Data;

public class StoreContextSeed
{
    public static async Task SeedAsync(StoreContext context, ILogger<StoreContextSeed> logger)
    {
        try
        {
           
            await SeedDataAsync<ProductBrand>(context.ProductBrands, "../Infrastructure/Data/SeedData/brands.json", "ProductBrands", context, logger);
            await SeedDataAsync<ProductType>(context.ProductTypes, "../Infrastructure/Data/SeedData/types.json", "ProductTypes", context, logger);
            await context.SaveChangesAsync();

            await SeedDataAsync<Product>(context.Products, "../Infrastructure/Data/SeedData/products.json", "Products", context, logger);
            await SeedDataAsync<User>(context.Users, "../Infrastructure/Data/SeedData/users.json", "Users", context, logger);
            await SeedDataAsync<Customer>(context.Customers, "../Infrastructure/Data/SeedData/customers.json", "Customers", context, logger);
            await context.SaveChangesAsync();

            await SeedDataAsync<Order>(context.Orders, "../Infrastructure/Data/SeedData/orders.json", "Orders", context, logger);
            await context.SaveChangesAsync();

            await SeedDataAsync<OrderItem>(context.OrderItems, "../Infrastructure/Data/SeedData/orderItems.json", "OrderItems", context, logger);
            await SeedDataAsync<Payment>(context.Payments, "../Infrastructure/Data/SeedData/payments.json", "Payments", context, logger);
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred during seeding");
            throw;
        }
    }

    private static async Task SeedDataAsync<T>(
        DbSet<T> dbSet, string filePath, string entityName, StoreContext context, ILogger logger) where T : class
    {
        if (!dbSet.Any())
        {
            logger.LogInformation($"Seeding {entityName}...");
            var jsonData = await File.ReadAllTextAsync(filePath);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters =
                {
                    new JsonStringEnumConverter(),
                    new DateTimeConverterUsingDateTimeUtc()
                }
            };

            var data = JsonSerializer.Deserialize<List<T>>(jsonData, options);

            if (data != null && data.Count > 0)
            {
                dbSet.AddRange(data);
                logger.LogInformation($"{entityName} seeded successfully.");
            }
            else
            {
                logger.LogWarning($"No {entityName} data found to seed.");
            }
        }
        else
        {
            logger.LogInformation($"{entityName} already exists in the database.");
        }
    }
}
