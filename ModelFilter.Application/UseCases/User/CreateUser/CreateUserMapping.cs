using AutoMapper;
using ModelFilter.Domain.Models;
using BC = BCrypt.Net.BCrypt;

namespace ModelFilter.Application.UseCases.User.CreateUser
{
    public class CreateUserMapping : Profile
    {
        public CreateUserMapping() 
        {
            CreateMap<CreateUserRequest, UserModel>()
                .ForMember(x => x.UserName, map => map.MapFrom(src => src.UserName))
                .ForMember(x => x.Name, map => map.MapFrom(src => src.Name))
                .ForMember(x => x.Password, map => map.MapFrom(src => BC.HashPassword(src.Password)));
        }
    }
}
