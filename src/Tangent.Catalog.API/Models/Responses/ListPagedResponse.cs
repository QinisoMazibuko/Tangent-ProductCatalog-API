using System;
using Tangent.Catalog.API.Domain.Models;
using Tangent.Catalog.API.Services.Dtos;

namespace Tangent.Catalog.API.Models.Responses;

public class ListPagedResponse<T> : OperationResult<T>
    where T : class
{
    public ProductFilterDto Filters { get; set; }

}
