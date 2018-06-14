using AutoMapper;
using SplitBillsBackend.Entities;
using SplitBillsBackend.Models;

namespace SplitBillsBackend.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterModel, User>();
            CreateMap<Category, CategoryModel>();
            CreateMap<Subcategory, SubcategoryModel>();

            CreateMap<UserBill, PayerModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.User.Id))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User.Name))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName));

            CreateMap<Bill, BillModel>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Subcategory.Category.Name))
                .ForMember(dest => dest.SubcategoryName, opt => opt.MapFrom(src => src.Subcategory.Name))
                .ForMember(dest => dest.Payers, opt => opt.MapFrom(src => src.UserBills));

            CreateMap<UserBill, BillModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Bill.Id))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Bill.Subcategory.Category.Name))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Bill.Date))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Bill.Description))
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Bill.Notes))
                .ForMember(dest => dest.SubcategoryName, opt => opt.MapFrom(src => src.Bill.Subcategory.Name));

            CreateMap<UserBill, UserBillModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Bill.Id))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Bill.Subcategory.Category.Name))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Bill.Date))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Bill.Description))
                .ForMember(dest => dest.Notes, opt => opt.MapFrom(src => src.Bill.Notes))
                .ForMember(dest => dest.SubcategoryName, opt => opt.MapFrom(src => src.Bill.Subcategory.Name));

            CreateMap<Friend, FriendModel>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.SecondFriend.Email))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.SecondFriend.Name))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.SecondFriend.PhoneNumber))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.SecondFriend.UserName));

            CreateMap<UserRole, RoleModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Role.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Role.Name));

            CreateMap<User, UserModel>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserRoles))
                .ForMember(dest => dest.Friends, opt => opt.MapFrom(src => src.Friends))
                .ForMember(dest => dest.Bills, opt => opt.MapFrom(src => src.UserBills));
        }
    }
}
