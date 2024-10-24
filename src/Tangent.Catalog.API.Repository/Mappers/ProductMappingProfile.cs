using System;
using AutoMapper;
using Tangent.Catalog.API.Domain.Entities;

namespace Tangent.Catalog.API.Repository.Mappers;

public class ProductMappingProfile : Profile
{
    public ProductMappingProfile()
    {
        AllowNullDestinationValues = true;
        CreateMap<Dtos.Product, Product>();
    }
}
