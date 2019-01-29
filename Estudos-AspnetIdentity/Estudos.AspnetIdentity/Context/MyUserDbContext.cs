using Estudos.AspnetIdentity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Estudos.AspnetIdentity.Context
{
    public class MyUserDbContext : IdentityDbContext<MyUser>
    {
        public MyUserDbContext(DbContextOptions<MyUserDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Organization>(org =>
            {
                org.ToTable("Organization");
                org.HasKey(x => x.Id);

                org.HasMany<MyUser>()
                    .WithOne()
                    .HasForeignKey(x => x.OrgId)
                    .IsRequired(false);
            });

        }
    }
}