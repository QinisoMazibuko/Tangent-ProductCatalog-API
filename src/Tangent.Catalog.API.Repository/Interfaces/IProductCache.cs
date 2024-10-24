using System;
using Tangent.Catalog.API.Domain.Entities;
namespace Tangent.Catalog.API.Repository.Interfaces;

public interface IProductCache
{
    Task<Product> GetProductAsync(string cacheKey, Func<Task<Product>> fetchFunc);
    Task<IEnumerable<Product>> GetProductsAsync(string cacheKey, Func<Task<IEnumerable<Product>>> fetchFunc);
    Task AddOrUpdateProductAsync(string cacheKey, Product product);
    Task RemoveProductAsync(string cacheKey);
    Task<IEnumerable<Product>> GetAllCachedProductsAsync();
}
