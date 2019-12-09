using Microsoft.AspNetCore.Identity;

namespace Estudos.AspNetIdentityJwt.Domain
{
    public class UserRole : IdentityUserRole<int>
    {
        public User User { get; set; }
        public Role Role { get; set; }
    }
}