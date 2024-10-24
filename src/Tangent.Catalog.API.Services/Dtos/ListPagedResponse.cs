using System;
using System.Runtime.Serialization;

namespace Tangent.Catalog.API.Services.Dtos;

[DataContract]
public class ListPagedResponse<T>
{
    [DataMember(Name = "filters")]
    public ProductFilterDto Filters { get; set; }

    [DataMember(Name = "metadata")]
    public MetadataDto Metadata { get; set; }

    [DataMember(Name = "results")]
    public T Results { get; set; }
}


[DataContract]
public class PagedResponseMetadata
{
    [DataMember(Name = "pageId")]
    public string PageId { get; set; }

    [DataMember(Name = "nextPageId")]
    public string NextPageId { get; set; }

    [DataMember(Name = "resultsPerPage")]
    public int ResultsPerPage { get; set; }

    [DataMember(Name = "totalResults")]
    public int TotalResults { get; set; }
}
