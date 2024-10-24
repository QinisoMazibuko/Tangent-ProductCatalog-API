using System;
using System.Runtime.Serialization;
using Tangent.Catalog.API.Domain.Constants;
using Tangent.Catalog.API.Services.Dtos;

namespace Tangent.Catalog.API.Models.Requests;

[DataContract]
public class ProductPagedRequest
{
    [DataMember(Name = "pageId")]
    public string? PageId { get; set; }

    [DataMember(Name = "resultsPerPage")]
    public int ResultsPerPage { get; set; } = Defaults.DEFAULT_PAGE_SIZE;

    [DataMember(Name = "filters")]
    public ProductFilterDto? Filters { get; set; }

}
