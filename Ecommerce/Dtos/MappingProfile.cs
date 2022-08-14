using AutoMapper;
using Ecommerce.Models;
using Ecommerce.Areas.Identity.Data;

namespace Ecommerce.Dtos
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Inventory, InventoryDto>();
            CreateMap<InventoryDto, Inventory>();

            CreateMap<Employee, EmployeeDto>();
            CreateMap<EmployeeDto, Employee>();

            CreateMap<TempCart, TempCartDto>();
            CreateMap<TempCartDto, TempCart>();
            
            CreateMap<Cart, CartDto>();
            CreateMap<CartDto, Cart>();

            CreateMap<Order, OrderDto>();
            CreateMap<OrderDto, Order>();

            CreateMap<EcommerceUser, EcommerceUserDto>();
            CreateMap<EcommerceUserDto, EcommerceUser>();
        }
    }
}
