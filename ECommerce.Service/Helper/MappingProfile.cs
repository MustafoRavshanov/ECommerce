using AutoMapper;
using ECommerce.Domain.DTOs;
using ECommerce.Domain.Entities;

namespace ECommerce.Service.Helper;

public class MappingProfile:Profile
{
    public MappingProfile() 
    {
        CreateMap<Customer, CustomerDto>().ReverseMap();
        CreateMap<Customer, CustomerFullInformationDto>()
            .ForMember(dest => dest.DistrictName,
               opt => opt.MapFrom(src => src.District.Name));
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
        CreateMap<Category, CategoryFullInformationDto>()
            .ForMember(dest => dest.Products,
               opt => opt.MapFrom(src => src.Products));
        CreateMap<CategoryCreateDto, Category>().ReverseMap();
        CreateMap<CategoryUpdateDto, Category>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<Order, OrderDto>().ReverseMap();
		CreateMap<Order, OrderFullInformationDto>()
	 .ForMember(dest => dest.DistrictName,
				opt => opt.MapFrom(src => src.District.Name))
	 .ForMember(dest => dest.RegionName,
				opt => opt.MapFrom(src => src.District.Region.Name))
	 .ForMember(dest => dest.CustomerFirstName,
				opt => opt.MapFrom(src => src.Customer.FirstName))
	 .ForMember(dest => dest.CustomerLastName,
				opt => opt.MapFrom(src => src.Customer.LastName))
	 .ForMember(dest => dest.CustomerPhoneNumber,
				opt => opt.MapFrom(src => src.Customer.PhoneNumber)) 
	 .ForMember(dest => dest.Items,
				opt => opt.MapFrom(src => src.OrderDetails));

		CreateMap<OrderCreateDto, Order>().ReverseMap();
        CreateMap<OrderUpdateDto, Order>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<Product, ProductFullDto>()
            .ForMember(dest => dest.CategoryNameUz, opt => opt.MapFrom(src => src.Category.NameUz));
        CreateMap<Product,ProductFullInformationDto>()
             .ForMember(dest => dest.CategoryNameUz, opt => opt.MapFrom(src => src.Category.NameUz));
        CreateMap<ProductCreateDto, Product>().ReverseMap();
        CreateMap<ProductUpdateDto, Product>()
    .ForMember(dest => dest.NameUz, opt => opt.Condition(src => src.NameUz != null))
    .ForMember(dest => dest.NameEn, opt => opt.Condition(src => src.NameEn != null))
    .ForMember(dest => dest.Price, opt => opt.Condition(src => src.Price != null))
    .ForMember(dest => dest.CategoryId, opt => opt.Condition(src => src.CategoryId != null))
    .ForMember(dest => dest.DescriptionUz, opt => opt.Condition(src => src.DescriptionUz != null))
    .ForMember(dest => dest.DescriptionEn, opt => opt.Condition(src => src.DescriptionEn != null))
    .ForMember(dest => dest.StockQuantity, opt => opt.Condition(src => src.StockQuantity != null))
    .ForMember(dest => dest.Weight, opt => opt.Condition(src => src.Weight != null))
    .ForMember(dest => dest.ImageUrl, opt => opt.Condition(src => src.ImageUrl != null));

        CreateMap<Basket, BasketItemDto>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.NameUz))
            .ForMember(dest => dest.ProductPrice, opt => opt.MapFrom(src => src.Product.Price))
            .ForMember(dest => dest.ProductImageUrl, opt => opt.MapFrom(src => src.Product.ImageUrl));
        CreateMap<BasketCreateDto, Basket>().ReverseMap();
        CreateMap<BasketUpdateDto, Basket>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        CreateMap<User, UserDto>()
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.Name));
        CreateMap<User, UserFullDto>()
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.Name))
            .ForMember(dest => dest.Permissions, opt => opt.MapFrom(src => src.Role.RolePermissions.Select(rp => rp.Permission).ToList()));
        CreateMap<UserCreateDto, User>();
        CreateMap<UserUpdateDto, User>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        CreateMap<UserUpdatePasswordDto, User>();

        CreateMap<Role, RoleDto>()
            .ForMember(dest => dest.Permissions, opt => opt.MapFrom(src => src.RolePermissions.Select(rp => rp.Permission).ToList()));
        CreateMap<RoleCreateDto, Role>();
        CreateMap<RoleUpdateDto, Role>();

        CreateMap<RegisterDto, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())  
            .ForMember(dest => dest.RoleId, opt => opt.Ignore())    
            .ForMember(dest => dest.IsActive, opt => opt.Ignore());

        CreateMap<OrderDetail, OrderDetailDto>()
            .ForMember(dest => dest.ProductNameUz, opt => opt.MapFrom(src => src.Product!.NameUz))
            .ForMember(dest => dest.ProductNameEn, opt => opt.MapFrom(src => src.Product!.NameEn));
    }
}
