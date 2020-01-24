using AutoMapper;
using CartService.Application.DTO;
using CartService.Domain.Entities;

namespace CartService.Application.Mappings
{
    public class AutomapperMappingProfile : Profile
    {
        public AutomapperMappingProfile()
        {
            CreateMap<ShoppingCart, ShoppingCartDTO>().ReverseMap();
            CreateMap<ShoppingCartItem, ShoppingCartItemDTO>().ReverseMap();
        }
    }
}
