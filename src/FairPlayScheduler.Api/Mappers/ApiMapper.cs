using AutoMapper;
using FairPlayScheduler.Api.Model;
using FairPlayScheduler.Api.Model.Api;

namespace FairPlayScheduler.Api.Mappers
{
    public class ApiMapper : Profile
    {
        public ApiMapper() 
        {
            CreateMap<Responsibility, ResponsibilityModel>()
                .ForMember(dest => dest.Cadence, act => act.MapFrom(src => Enum.GetName(typeof(Cadence), (int)src.Cadence)))
                .ForMember(dest => dest.Schedule, act => act.MapFrom(src => src.CronSchedule));

            CreateMap<ResponsibilityByDay, DailyResponsibilityModel>();
        }
    }
}
