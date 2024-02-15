using AutoMapper;
using Messenger.MessageManager.DataAccessLayer.Models;
using Messenger.Shared.Contracts.Messages.Responses;

namespace Messenger.MessageManager.BusinessLogicalLayer.Mappers;

public class MessageProfile : Profile
{
    public MessageProfile()
    {
        CreateMap<Message, MessageResponse>()
            .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
            .ForMember(dest => dest.SenderId, opt => opt.MapFrom(src => src.SenderId))
            ;
    }
}
