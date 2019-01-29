using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Estudos.AspnetIdentity.Models
{
    public class MyUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<MyUser>
    {
        public MyUserClaimsPrincipalFactory(UserManager<MyUser> userManager, IOptions<IdentityOptions> optionsAccessor)
            : base(userManager, optionsAccessor)
        {
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(MyUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            identity.AddClaim(new Claim("Member", user.Member));
            return identity;
        }
    }
}