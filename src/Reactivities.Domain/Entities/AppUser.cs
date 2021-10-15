using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Reactivities.Domain.Entities
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }
        public string Bio { get; set; }
        public ICollection<ActivityAttendee> ActivityAttendees { get; set; }
    }
}
