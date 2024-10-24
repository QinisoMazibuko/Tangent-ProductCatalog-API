using Tangent.Catalog.API.Domain.Models;
using Tangent.Catalog.API.Services.Dtos;

namespace Tangent.Catalog.API.Services.Interfaces;

public interface IProductService
{
    Task<long> CreateProductAsync(ProductDto product);

    Task<ProductDto> UpdateProductAsync(ProductDto product);

    Task<OperationResult<IEnumerable<ProductDto>>> GetProducts(int resultsPerPage, ProductFilterDto productFilters, string pageId);

    Task<ProductDto> GetProductByIdAsync(long id);

    Task<bool> DeleteProduct(long productId);

}
