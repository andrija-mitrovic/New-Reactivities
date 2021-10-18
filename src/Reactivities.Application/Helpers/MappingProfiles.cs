using AutoMapper;
using Reactivities.Application.DTOs;
using Reactivities.Domain.Entities;
using System.Linq;

namespace Reactivities.Application.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Activity, Activity>();
            CreateMap<Activity, ActivityDto>()
                .ForMember(x => x.Profiles, y => y.MapFrom(z => z.ActivityAttendees))
                .ForMember(x => x.HostUsername,
                    y => y.MapFrom(z => z.ActivityAttendees.FirstOrDefault(w => w.IsHost).AppUser.UserName));
            CreateMap<ActivityAttendee, ProfileDto>()
                .ForMember(x => x.DisplayName, y => y.MapFrom(z => z.AppUser.DisplayName))
                .ForMember(x => x.Username, y => y.MapFrom(z => z.AppUser.UserName))
                .ForMember(x => x.Bio, y => y.MapFrom(z => z.AppUser.Bio));
        }
    }
}
