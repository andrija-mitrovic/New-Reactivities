using AutoMapper;
using Reactivities.Application.DTOs;
using Reactivities.Domain.Entities;

namespace Reactivities.Application.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Activity, Activity>();
            CreateMap<Activity, ActivityDto>();
        }
    }
}
