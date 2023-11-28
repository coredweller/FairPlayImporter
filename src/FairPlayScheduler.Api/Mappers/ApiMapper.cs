using AutoMapper;
using FairPlayScheduler.Api.Model;
using FairPlayScheduler.Api.Model.Api;
using FairPlayScheduler.Api.Model.Notifications;

namespace FairPlayScheduler.Api.Mappers
{
    public class ApiMapper : Profile
    {
        public ApiMapper() 
        {
            CreateMap<Responsibility, ResponsibilityResponse>()
                .ForMember(dest => dest.Cadence, act => act.MapFrom(src => Enum.GetName(typeof(Cadence), (int)src.Cadence)))
                .ForMember(dest => dest.Schedule, act => act.MapFrom(src => src.CronSchedule))
                .ForMember(dest => dest.IsCompleted, act => act.MapFrom(src => src.MarkAsComplete));

            CreateMap<Responsibility, ResponsibilityEmailModel>()
                .ForMember(dest => dest.CadenceName, act => act.MapFrom(src => Enum.GetName(typeof(Cadence), (int)src.Cadence)))
                .ForMember(dest => dest.Schedule, act => act.MapFrom(src => src.CronSchedule))
                .ForMember(dest => dest.IsCompleted, act => act.MapFrom(src => src.MarkAsComplete));

            CreateMap<ResponsibilityByDay, DailyResponsibilityResponse>();
            CreateMap<ResponsibilityByDay, ResponsibilityByDateEmailModel>();
            CreateMap<DailyResponsibilityRequest, ResponsibilityByDay>();
            CreateMap<ResponsibilityRequest, Responsibility>();
            CreateMap<SendResponsibilitiesEmailRequest, EmailAuthorizationSettings>()
                .ForMember(dest => dest.UserName, act => act.MapFrom(src => src.SenderUserName))
                .ForMember(dest => dest.Password, act => act.MapFrom(src => src.SenderPassword));
            CreateMap<DailyResponsibilityRequest, ResponsibilityByDay>();
            CreateMap<ResponsibilityRequest, Responsibility>()
                .ForMember(dest => dest.Cadence, act => act.MapFrom(src => GetCadenceFromName(src.Cadence)))
                .ForMember(dest => dest.CronSchedule, act => act.MapFrom(src => src.Schedule))
                .ForMember(dest => dest.MarkAsComplete, act => act.MapFrom(src => src.IsCompleted));

            CreateMap<CompletedTaskRequest, CompletedTask>();

            CreateMap<CompletedTask, CompletedTaskResponse>();
        }

        private Cadence GetCadenceFromName(string? name)
        {
            Cadence cadence;
            if(!Enum.TryParse(name, out cadence))
            {
                cadence = Cadence.Unknown;
            }
            return cadence;
        }
    }
}
