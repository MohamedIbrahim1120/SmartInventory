using AutoMapper;
using Domain.Entities;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Shared.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDto>()
               .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
               .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.Supplier.Name))
               .ReverseMap();

            CreateMap<ProductDto, Product>();

            CreateMap<Category, CategoryDto>().ReverseMap();

            CreateMap<Supplier, SupplierDto>().ReverseMap();


        }
    }
}
