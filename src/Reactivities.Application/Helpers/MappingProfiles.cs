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
            string currentUsername = null;
            CreateMap<Activity, Activity>();
            CreateMap<Activity, ActivityDto>()
                .ForMember(x => x.Profiles, y => y.MapFrom(z => z.ActivityAttendees))
                .ForMember(x => x.HostUsername,
                    y => y.MapFrom(z => z.ActivityAttendees.FirstOrDefault(w => w.IsHost).AppUser.UserName));
            CreateMap<ActivityAttendee, AttendeeDto>()
                .ForMember(x => x.DisplayName, y => y.MapFrom(z => z.AppUser.DisplayName))
                .ForMember(x => x.Username, y => y.MapFrom(z => z.AppUser.UserName))
                .ForMember(x => x.Bio, y => y.MapFrom(z => z.AppUser.Bio))
                .ForMember(x => x.Image, 
                    y => y.MapFrom(z => z.AppUser.Photos.FirstOrDefault(w => w.IsMain).Url))
                .ForMember(x => x.FollowersCount, y => y.MapFrom(z => z.AppUser.Followers.Count))
                .ForMember(x => x.FollowingCount, y => y.MapFrom(z => z.AppUser.Followings.Count))
                .ForMember(x => x.Following, 
                    y => y.MapFrom(z => z.AppUser.Followers.Any(w => w.Observer.UserName == currentUsername)));
            CreateMap<AppUser, ProfileDto>()
                .ForMember(x => x.Image, y => y.MapFrom(z => z.Photos.FirstOrDefault(w => w.IsMain).Url))
                .ForMember(x => x.FollowersCount, y => y.MapFrom(z => z.Followers.Count))
                .ForMember(x => x.FollowingCount, y => y.MapFrom(z => z.Followings.Count))
                .ForMember(x => x.Following, 
                    y => y.MapFrom(z => z.Followers.Any(w => w.Observer.UserName == currentUsername)));
            CreateMap<Comment, CommentDto>()
                .ForMember(x => x.DisplayName, y => y.MapFrom(z => z.Author.DisplayName))
                .ForMember(x => x.Username, y => y.MapFrom(z => z.Author.UserName))
                .ForMember(x => x.Image, y => y.MapFrom(z => z.Author.Photos.FirstOrDefault(w => w.IsMain).Url));
        }
    }
}
