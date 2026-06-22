using AutoMapper;
using ECommerce.Domain.DTOs;
using ECommerce.Domain.Entities;

namespace ECommerce.Service.Helper;

public class MappingProfile:Profile
{
    public MappingProfile() 
    {
        CreateMap<Customer, CustomerDto>().ReverseMap();
        CreateMap<CustomerCreateDto, Customer>();
        CreateMap<CustomerUpdateDto, Customer>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<Region, RegionDto>().ReverseMap();
        CreateMap<RegionCreateDto, Region>();
        CreateMap<RegionUpdateDto, Region>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<District, DistrictDto>()
            .ForMember(dest => dest.RegionName, opt => opt.MapFrom(src => src.Region.Name));
        CreateMap<DistrictCreateDto, District>();
        CreateMap<DistrictUpdateDto, District>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<Category, CategoryDto>().ReverseMap();
        CreateMap<CategoryCreateDto, Category>().ReverseMap();
        CreateMap<CategoryUpdateDto, Category>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<Order, OrderDto>().ReverseMap();
        CreateMap<OrderCreateDto, Order>().ReverseMap();
        CreateMap<OrderUpdateDto, Order>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.CategoryNameUz, opt => opt.MapFrom(src => src.Category.NameUz));
        CreateMap<ProductCreateDto, Product>().ReverseMap();
        CreateMap<ProductUpdateDto, Product>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<Basket, BasketItemDto>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.NameUz))
            .ForMember(dest => dest.ProductPrice, opt => opt.MapFrom(src => src.Product.Price))
            .ForMember(dest => dest.ProductImageUrl, opt => opt.MapFrom(src => src.Product.ImageUrl));
        CreateMap<BasketCreateDto, Basket>().ReverseMap();
        CreateMap<BasketUpdateDto, Basket>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
    }
}
