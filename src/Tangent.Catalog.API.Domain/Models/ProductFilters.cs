using System;

namespace Tangent.Catalog.API.Domain.Models;

public class ProductFilters
{
    public IEnumerable<long>? ProductIds { get; set; }

    public RangeFilter? PriceRange { get; set; }
}
