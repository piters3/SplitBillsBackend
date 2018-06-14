using AutoMapper;
using SplitBillsBackend.Entities;
using SplitBillsBackend.Models;
using System.Collections.Generic;

namespace SplitBillsBackend.Mappings
{
    public class ModelToEntityMappingProfile : Profile
    {
        public ModelToEntityMappingProfile()
        {
            CreateMap<RegisterModel, User>();
            CreateMap<Category, CategoryModel>();
              //.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id)); //gdyby były inne nazwy kluczy głównych

            CreateMap<Subcategory, SubcategoryModel>();
        }
    }
}
