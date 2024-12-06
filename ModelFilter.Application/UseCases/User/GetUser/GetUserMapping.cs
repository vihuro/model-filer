using AutoMapper;
using ModelFilter.Domain.Models;

namespace ModelFilter.Application.UseCases.User.GetUser
{
    public class GetUserMapping : Profile
    {
        public GetUserMapping()
        {
            CreateMap<ReturnDefault<UserModel>, ReturnDefault<UserReturnDefault>>()
                .ForMember(x => x.CurrentPage, map => map.MapFrom(src => src.CurrentPage))
                .ForMember(x => x.TotalPages, map => map.MapFrom(src => src.TotalPages))
                .ForMember(x => x.MaxPerPage, map => map.MapFrom(src => src.MaxPerPage))
                .ForMember(x => x.Errors, map => map.MapFrom(src => src.Errors))
                .ForMember(x => x.Sucess, map => map.MapFrom(src => src.Sucess))
                .ForMember(x => x.DataResult, map => map.MapFrom(src => src.DataResult.Select(x => new UserReturnDefault
                {
                    DateCreated = x.DateCreated,
                    UserName = x.UserName,
                    Name = x.Name,
                    DateUpdated = x.DateUpdated,
                    Id = x.Id
                })));
        }
    }
}
