using Estudos.AspNetIdentityJwt.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Estudos.AspNetIdentityJwt.Repository
{
    public class IdentityContext : IdentityDbContext<User, Role, int,
        IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public IdentityContext(DbContextOptions<IdentityContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserRole>(userRole =>
            {
                userRole.ToTable("UserRole");
                userRole.HasKey(lnq => new {lnq.UserId, lnq.RoleId});

                userRole.HasOne(lnq => lnq.Role)
                    .WithMany(lnq => lnq.UserRoles)
                    .HasForeignKey(lnq => lnq.RoleId);

                userRole.HasOne(lnq => lnq.User)
                    .WithMany(lnq => lnq.UserRoles)
                    .HasForeignKey(lnq => lnq.UserId);
            });
        }
    }
}