using System;
using System.Runtime.Serialization;

namespace Tangent.Catalog.API.Services.Dtos;

public class MetadataDto
{
    [DataMember(Name = "totalResults")]
    public int TotalResults { get; set; }

    [DataMember(Name = "resultsPerPage")]
    public int ResultsPerPage { get; set; }

    [DataMember(Name = "nextPageId")]
    public string NextPageId { get; set; }

}
