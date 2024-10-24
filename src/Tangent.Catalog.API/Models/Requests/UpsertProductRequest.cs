using System;
using Tangent.Catalog.API.Services.Dtos;

namespace Tangent.Catalog.API.Models.Requests;

public class UpsertProductRequest
{
    public ProductDto Product { get; set; }
}
