using Ambev.DeveloperEvaluation.Application.Commands.Sales;
using Ambev.DeveloperEvaluation.Application.Commands.SaleItems;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Mappings;

public class SaleMappingProfile : Profile
{
    public SaleMappingProfile()
    {
        CreateMap<Sale, SaleResponse>()
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.SaleDate));

        CreateMap<SaleItem, SaleItemResponse>()
            .ForMember(dest => dest.Discount, opt => opt.MapFrom(
                src => src.UnitPrice * src.Quantity * (src.DiscountPercentage / 100m)
            ));
    }
}
