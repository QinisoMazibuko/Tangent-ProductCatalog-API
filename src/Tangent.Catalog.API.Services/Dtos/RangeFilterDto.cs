using System.Runtime.Serialization;

namespace Tangent.Catalog.API.Services.Dtos;

[DataContract]
public class RangeFilterDto
{
    [DataMember(Name = "min")]
    public double? Min { get; set; }

    [DataMember(Name = "max")]
    public double? Max { get; set; }
}
