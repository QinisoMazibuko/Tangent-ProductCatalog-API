using System;
using LazyCache;
using Tangent.Catalog.API.Domain.Entities;
using Tangent.Catalog.API.Repository.Interfaces;

namespace Tangent.Catalog.API.Repository.Cache;

public class ProductCache : IProductCache
{
    private readonly IAppCache _cache;
    private const string _productKeysCacheKey = "ProductKeys";

    public ProductCache(IAppCache cache)
    {
        _cache = cache;
    }

    public async Task<Product> GetProductAsync(string cacheKey, Func<Task<Product>> fetchFunc)
    {

        if (_cache.TryGetValue(cacheKey, out Product resullt))
        {
            return resullt;
        }

        return null;
    }

    public async Task<IEnumerable<Product>> GetProductsAsync(string cacheKey, Func<Task<IEnumerable<Product>>> fetchFunc)
    {
        return await _cache.GetOrAddAsync(cacheKey, fetchFunc, DateTimeOffset.Now.AddHours(1));
    }

    // Add or update a product in the cache
    public async Task AddOrUpdateProductAsync(string cacheKey, Product product)
    {
        var productKeys = await GetProductKeysAsync();
        if (!productKeys.Contains(cacheKey))
        {
            productKeys.Add(cacheKey);
            _cache.Add(_productKeysCacheKey, productKeys, DateTimeOffset.Now.AddHours(1));
        }

        _cache.Add(cacheKey, product, DateTimeOffset.Now.AddHours(1));
    }

    public Task RemoveProductAsync(string cacheKey)
    {
        _cache.Remove(cacheKey);
        return Task.CompletedTask;
    }

    // Get all product keys
    private async Task<List<string>> GetProductKeysAsync()
    {
        return await _cache.GetOrAddAsync(_productKeysCacheKey, () => Task.FromResult(new List<string>()));
    }

    // Get all products in the cache
    public async Task<IEnumerable<Product>> GetAllCachedProductsAsync()
    {
        var productKeys = await GetProductKeysAsync();
        var products = new List<Product>();

        foreach (var key in productKeys)
        {
            var product = await _cache.GetAsync<Product>(key);
            if (product != null)
            {
                products.Add(product);
            }
        }

        return products;
    }
}

