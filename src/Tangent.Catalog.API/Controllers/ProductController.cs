using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Tangent.Catalog.API.Models.Requests;
using Tangent.Catalog.API.Models.Responses;
using Tangent.Catalog.API.Services.Dtos;
using Tangent.Catalog.API.Services.Interfaces;

namespace Tangent.Catalog.API.Controllers;


[ApiController]
[Route("api/v1/[controller]")]
public class ProductController : Controller
{

    private readonly IProductService _productService;
    private readonly ILogger<ProductController> _logger;

    public ProductController(IProductService productService, ILogger<ProductController> logger)
    {
        _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        _logger = logger;
    }

    /// <summary>
    /// Get a single Product by Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}", Name = "GetProductById")]
    public async Task<IActionResult> GetProductById(long id)
    {

        var result = await _productService.GetProductByIdAsync(id);

        if (result == null)
            return NotFound();

        return Ok(result);
    }


    /// <summary>
    /// Get a list of Catalog Products
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("search", Name = "GetProducts")]
    public async Task<IActionResult> GetProducts([FromBody] ProductPagedRequest request)
    {
        try
        {
            var result = await _productService.GetProducts(request.ResultsPerPage, request.Filters, request.PageId);

            if (result == null)
            {
                return NotFound();
            }

            var response = new Models.Responses.ListPagedResponse<IEnumerable<ProductDto>>
            {
                Filters = request.Filters,
                Metadata = result.Metadata,
                Results = result.Results,
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }

    /// <summary>
    /// Create a new Product in the Catalog
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost(Name = "CreateProduct")]
    public async Task<IActionResult> CreateProduct([FromBody] UpsertProductRequest request)
    {
        try
        {
            var result = await _productService.CreateProductAsync(request.Product);

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest("Unable to Create Product: " + ex.Message);
        }
    }



    /// <summary>
    /// Update A Product 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct([FromBody] UpsertProductRequest request, long id)
    {
        var result = await _productService.UpdateProductAsync(request.Product);

        if (result == null) return BadRequest("Unable to update Product");

        return Ok(result);
    }


    /// <summary>
    /// Delete a Product
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(long id)
    {
        var result = await _productService.DeleteProduct(id);

        if (!result) return BadRequest("Unable to delete Product");

        return Ok(result);
    }

}
