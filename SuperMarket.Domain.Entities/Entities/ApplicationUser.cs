using Microsoft.AspNetCore.Identity;
using SuperMarket.Domain.Enums;

namespace SuperMarket.Domain.Entities.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Gender Gender { get; set; }

        public bool IsActive { get; set; }

        public string? RefreshToken { get; set; }

        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
