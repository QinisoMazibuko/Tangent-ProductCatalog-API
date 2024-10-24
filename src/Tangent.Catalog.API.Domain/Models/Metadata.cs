using System;

namespace Tangent.Catalog.API.Domain.Models;

public class Metadata
{
    public int TotalResults { get; set; }

    public int ResultsPerPage { get; set; }

    public string NextPageId { get; set; }
}
