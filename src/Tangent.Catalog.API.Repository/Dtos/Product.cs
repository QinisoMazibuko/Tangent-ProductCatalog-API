using System;

namespace Tangent.Catalog.API.Repository.Dtos;

public class Product
{
    public long Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public decimal Price { get; set; }

    public int Stock { get; set; }
}
