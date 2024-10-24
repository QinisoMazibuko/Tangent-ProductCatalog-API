using AutoMapper;
using Tangent.Catalog.API.Domain.Entities;
using Tangent.Catalog.API.Domain.Models;
using Tangent.Catalog.API.Services.Dtos;

namespace Tangent.Catalog.API.Services.Mappers;

public class DomainMappingProfile : Profile
{
    public DomainMappingProfile()
    {
        CreateMap<ProductDto, Product>().ReverseMap();
        CreateMap<MetadataDto, Metadata>();
        CreateMap<ProductFilterDto, ProductFilters>();
        CreateMap<RangeFilterDto, RangeFilter>();
    }
}
