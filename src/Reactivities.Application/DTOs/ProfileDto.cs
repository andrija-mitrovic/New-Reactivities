using Reactivities.Domain.Entities;
using System.Collections.Generic;

namespace Reactivities.Application.DTOs
{
    public class ProfileDto
    {
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string Bio { get; set; }
        public string Image { get; set; }
        public ICollection<Photo> Photos { get; set; }
    }
}
