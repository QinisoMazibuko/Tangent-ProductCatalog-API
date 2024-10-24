using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Tangent.Catalog.API.Services.Dtos;

[DataContract]
public class ProductFilterDto
{
    [DataMember(Name = "ProductIds")]
    public IEnumerable<long>? ProductIds { get; set; }


    [DataMember(Name = "PriceRange")]
    public RangeFilterDto? PriceRange { get; set; }

}
