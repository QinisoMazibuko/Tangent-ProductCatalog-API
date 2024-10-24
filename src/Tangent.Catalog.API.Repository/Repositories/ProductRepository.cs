using System;
using System.Xml.XPath;
using Tangent.Catalog.API.Domain.Entities;
using Tangent.Catalog.API.Domain.Models;
using Tangent.Catalog.API.Repository.Interfaces;

namespace Tangent.Catalog.API.Repository.Repositories;

public class ProductRepository : IProductRepository
{

    private readonly IProductCache _productCache;

    public ProductRepository(IProductCache productCache)
    {
        _productCache = productCache ?? throw new ArgumentNullException(nameof(productCache));
    }


    public async Task<Product> CreateProductAsync(Product product)
    {
        if (product.Id == 0)
        {
            product.Id = GenerateNewProductId();
        }

        var cacheKey = $"Product-{product.Id}";
        await _productCache.AddOrUpdateProductAsync(cacheKey, product);

        return product;
    }

    public async Task<bool> DeleteProductAsync(long productId)
    {
        var cacheKey = $"Product-{productId}";
        await _productCache.RemoveProductAsync(cacheKey);

        return true;
    }

    public async Task<Product> GetProductByIdAsync(long id)
    {
        var cacheKey = $"Product-{id}";
        var result = await _productCache.GetProductAsync(cacheKey, () =>
        {
            return null;
        });

        if (result == null)
        {
            throw new KeyNotFoundException();
        }

        return result;
    }

    public async Task<Product> UpdateProductAsync(Product product)
    {
        // Simulate updating in the database (you can replace this with actual database logic)
        var cacheKey = $"Product-{product.Id}";
        await _productCache.AddOrUpdateProductAsync(cacheKey, product);

        return product;
    }

    public async Task<(IEnumerable<Product>, string nextPageId, int total)> GetProductsAsync(ProductFilters filters, int resultsPerPage, string pageId)
    {
        var allProducts = await _productCache.GetAllCachedProductsAsync();
        int totalResult = allProducts.Count();
        // Apply filtering based on ProductFilterDto
        if (filters != null)
        {
            if (filters.ProductIds != null && filters.ProductIds.Any())
            {
                allProducts = allProducts.Where(p => filters.ProductIds.Contains(p.Id));
            }

            if (filters.PriceRange != null)
            {
                allProducts = allProducts.Where(p => (double)p.Price >= filters.PriceRange.Min && (double)p.Price <= filters.PriceRange.Max);
            }
        }

        // Apply pagination
        var productList = allProducts.ToList();
        var pageNumber = !string.IsNullOrEmpty(pageId) ? int.Parse(pageId) : 1;
        var pagedProducts = productList.Skip((pageNumber - 1) * resultsPerPage).Take(resultsPerPage).ToList();

        // Determine next page ID
        string nextPageId = null;
        if (pageNumber * resultsPerPage < productList.Count)
        {
            nextPageId = (pageNumber + 1).ToString();

        }

        totalResult = productList.Count();

        return (pagedProducts, nextPageId, totalResult);
    }


    #region  Helper functions 

    private int GenerateNewProductId()
    {
        var cachedProducts = _productCache.GetAllCachedProductsAsync().Result;

        if (cachedProducts != null && cachedProducts.Any())
        {
            return (int)(cachedProducts.Max(p => p.Id) + 1); // Increment the highest product ID
        }

        return 1;
    }
    #endregion

}
