using AutoMapper;
using ServiceStack.ServiceInterface.ServiceModel;
using System;

namespace Phytel.Services.Journal.Map
{
    public class ErrorMap : Profile
    {
        protected override void Configure()
        {
            CreateMap<Exception, Error>()
                .ForMember(result => result.ErrorMessage, opt => opt.MapFrom(source => source.Message))
                .ForMember(result => result.StackTrace, opt => opt.MapFrom(source => source.StackTrace))
                .ForMember(result => result.InnerError, opt => opt.MapFrom(source => Mapper.Map<Error>(source.InnerException)));

            CreateMap<Exception, ResponseStatus>()
                .ForMember(result => result.ErrorCode, opt => opt.MapFrom(source => source.GetType().Name))
                .ForMember(result => result.Message, opt => opt.MapFrom(source => source.Message))
                .ForMember(result => result.StackTrace, opt => opt.MapFrom(source => source.StackTrace));

            CreateMap<Exception, ErrorResponse>()
                .ForMember(result => result.ResponseStatus, opt => opt.MapFrom(source => Mapper.Map<ResponseStatus>(source)));
        }
    }
}