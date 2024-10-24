using System;
using Tangent.Catalog.API.Domain.Entities;
using Tangent.Catalog.API.Domain.Models;
namespace Tangent.Catalog.API.Repository.Interfaces;

public interface IProductRepository
{
    Task<Product> CreateProductAsync(Product product);
    Task<Product> UpdateProductAsync(Product product);
    Task<bool> DeleteProductAsync(long productId);
    Task<Product> GetProductByIdAsync(long id);
    Task<(IEnumerable<Product>, string nextPageId, int total)> GetProductsAsync(ProductFilters filters, int resultsPerPage, string pageId);
}
