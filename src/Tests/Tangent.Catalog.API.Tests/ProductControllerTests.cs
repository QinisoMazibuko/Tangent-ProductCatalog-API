using Microsoft.AspNetCore.Mvc;
using Moq;
using Tangent.Catalog.API.Controllers;
using Tangent.Catalog.API.Models.Requests;
using Tangent.Catalog.API.Services.Dtos;
using Tangent.Catalog.API.Services.Interfaces;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tangent.Catalog.API.Controllers;
using Tangent.Catalog.API.Services.Interfaces;


namespace Tangent.Catalog.API.Tests;

public class ProductControllerTests
{
    private readonly ProductController _productController;
    private readonly Mock<IProductService> _mockService;
    public ProductControllerTests()
    {
        _mockService = new Mock<IProductService>();

        _productController = new ProductController(_mockService.Object);
    }


    [Fact]
    public async Task GetProductById_ExistingProduct_ReturnsOkResponse()
    {
        // Arrange
        long productId = 1;
        var mockProduct = new ProductDto { Id = productId, Name = "Product 1" };
        _mockService.Setup(x => x.GetProductByIdAsync(productId)).ReturnsAsync(mockProduct);

        // Act
        var response = await _productController.GetProductById(productId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(response);
        Assert.Equal(mockProduct, okResult.Value);
    }

    [Fact]
    public async Task GetProductById_NonExistingProduct_ReturnsNotFound()
    {
        // Arrange
        long productId = 1;
        _mockService.Setup(x => x.GetProductByIdAsync(productId)).ReturnsAsync((ProductDto)null);

        // Act
        var response = await _productController.GetProductById(productId);

        // Assert
        Assert.IsType<NotFoundResult>(response);
    }

    [Fact]
    public async Task GetProducts_ValidRequest_ReturnsOkResponse()
    {
        // Arrange
        var request = new ProductPagedRequest
        {
            ResultsPerPage = 10,
            Filters = new ProductFilterDto(),
            PageId = "1"
        };
        var mockResult = new Domain.Models.OperationResult<IEnumerable<Tangent.Catalog.API.Services.Dtos.ProductDto>>
        {
            Metadata = new Domain.Models.Metadata { TotalResults = 1, ResultsPerPage = 10 },
            Results = new List<ProductDto> { new ProductDto { Id = 1, Name = "Product 1" } }
        };
        _mockService.Setup(x => x.GetProducts(request.ResultsPerPage, request.Filters, request.PageId)).ReturnsAsync(mockResult);

        // Act
        var response = await _productController.GetProducts(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(response);
        var result = Assert.IsType<ListPagedResponse<IEnumerable<ProductDto>>>(okResult.Value);
        Assert.NotNull(result);
        Assert.Equal(1, result.Metadata.TotalResults);
    }

    [Fact]
    public async Task CreateProduct_ValidProduct_ReturnsOkResponse()
    {
        // Arrange
        var request = new UpsertProductRequest
        {
            Product = new ProductDto { Name = "New Product" }
        };
        var mockProduct = new ProductDto { Id = 1, Name = "New Product" };
        _mockService.Setup(x => x.CreateProductAsync(request.Product)).ReturnsAsync(mockProduct.Id);

        // Act
        var response = await _productController.CreateProduct(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(response);
        Assert.Equal(mockProduct, okResult.Value);
    }

    [Fact]
    public async Task UpdateProduct_ValidProduct_ReturnsOkResponse()
    {
        // Arrange
        var productId = 1;
        var request = new UpsertProductRequest
        {
            Product = new ProductDto { Id = productId, Name = "Updated Product" }
        };
        var mockProduct = new ProductDto { Id = productId, Name = "Updated Product" };
        _mockService.Setup(x => x.UpdateProductAsync(request.Product)).ReturnsAsync(mockProduct);

        // Act
        var response = await _productController.UpdateProduct(request, productId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(response);
        Assert.Equal(mockProduct, okResult.Value);
    }

    [Fact]
    public async Task UpdateProduct_InvalidProduct_ReturnsBadRequest()
    {
        // Arrange
        var productId = 1;
        var request = new UpsertProductRequest
        {
            Product = new ProductDto { Id = productId, Name = "Updated Product" }
        };
        _mockService.Setup(x => x.UpdateProductAsync(request.Product)).ReturnsAsync((ProductDto)null);

        // Act
        var response = await _productController.UpdateProduct(request, productId);

        // Assert
        Assert.IsType<BadRequestObjectResult>(response);
    }

    [Fact]
    public async Task DeleteProduct_ValidProduct_ReturnsOkResponse()
    {
        // Arrange
        var productId = 1;
        _mockService.Setup(x => x.DeleteProduct(productId)).ReturnsAsync(true);

        // Act
        var response = await _productController.DeleteProduct(productId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(response);
        Assert.True((bool)okResult.Value);
    }

    [Fact]
    public async Task DeleteProduct_InvalidProduct_ReturnsBadRequest()
    {
        // Arrange
        var productId = 1;
        _mockService.Setup(x => x.DeleteProduct(productId)).ReturnsAsync(false);

        // Act
        var response = await _productController.DeleteProduct(productId);

        // Assert
        Assert.IsType<BadRequestObjectResult>(response);
    }
}

