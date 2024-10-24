using System;

namespace Tangent.Catalog.API.Services.Dtos;

public class ProductDto
{
    public long Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public decimal Price { get; set; }

    public int Stock { get; set; }
}
