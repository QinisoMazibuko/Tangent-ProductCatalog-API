namespace Tangent.Catalog.API.Domain.Models;

public class OperationResult<T>
       where T : class
{
    public Metadata? Metadata { get; set; }

    public T Results { get; set; }
}
