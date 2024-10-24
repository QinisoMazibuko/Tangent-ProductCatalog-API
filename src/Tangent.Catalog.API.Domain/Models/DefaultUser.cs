using System;

namespace Tangent.Catalog.API.Domain.Models;

public class DefaultUser
{
    public required string SubscriberId { get; set; }

    public required string Name { get; set; }

    public required string Email { get; set; }

}
