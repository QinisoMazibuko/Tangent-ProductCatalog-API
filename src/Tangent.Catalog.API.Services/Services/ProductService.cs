using System;
using Tangent.Catalog.API.Domain.Models;
using Tangent.Catalog.API.Repository.Interfaces;
using Tangent.Catalog.API.Services.Dtos;
using Tangent.Catalog.API.Services.Interfaces;
using AutoMapper;
using Tangent.Catalog.API.Domain.Entities;
namespace Tangent.Catalog.API.Services.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<bool> DeleteProduct(long productId)
    {
        return await _productRepository.DeleteProductAsync(productId);
    }

    public async Task<ProductDto> GetProductByIdAsync(long id)
    {
        var product = await _productRepository.GetProductByIdAsync(id);

        if (product == null) return null;

        return _mapper.Map<ProductDto>(product);
    }

    public async Task<OperationResult<IEnumerable<ProductDto>>> GetProducts(int resultsPerPage, ProductFilterDto productFilters, string pageId)
    {
        var domainFilters = _mapper.Map<ProductFilters>(productFilters);


        var (products, nextPageId, total) = await _productRepository.GetProductsAsync(domainFilters, resultsPerPage, pageId);
        var mappedProducts = _mapper.Map<IEnumerable<ProductDto>>(products);

        return new OperationResult<IEnumerable<ProductDto>>()
        {
            Results = mappedProducts,
            Metadata = new Metadata
            {
                NextPageId = nextPageId,
                ResultsPerPage = resultsPerPage,
                TotalResults = total
            }
        };
    }

    public async Task<ProductDto> UpdateProductAsync(ProductDto productDto)
    {
        var product = _mapper.Map<Product>(productDto);
        var updatedProduct = await _productRepository.UpdateProductAsync(product);
        return _mapper.Map<ProductDto>(updatedProduct);
    }

    public async Task<long> CreateProductAsync(ProductDto product)
    {
        var prod = _mapper.Map<Product>(product);
        var createdProduct = await _productRepository.CreateProductAsync(prod);
        return createdProduct.Id;
    }
}

