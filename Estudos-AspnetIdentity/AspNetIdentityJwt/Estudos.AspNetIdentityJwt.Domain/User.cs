using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Estudos.AspNetIdentityJwt.Domain
{
    public class User : IdentityUser<int>
    {
        public string NomeCompleto { get; set; }
        public string Member { get; set; } = "Member";

        public IEnumerable<UserRole> UserRoles { get; set; }
    }
}