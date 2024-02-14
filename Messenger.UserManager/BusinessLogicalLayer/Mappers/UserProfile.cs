using AutoMapper;
using Messenger.Shared.Contracts.Users.Responses;
using Messenger.UserManager.Domain;

namespace Messenger.UserManager.BusinessLogicalLayer.Mappers;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserResponse>()
            .ForMember(destination => destination.Id, opt => opt.MapFrom(source => source.Id))
            .ForMember(destination => destination.Email, opt => opt.MapFrom(source => source.Email))
            .ForMember(destination => destination.Role, opt => opt.MapFrom(source => source.Role!.ToString()))
            ;
    }
}
