using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SuperMarket.Domain.Entities.Common;
using SuperMarket.Domain.Entities.Entities;
using SuperMarket.Services.Interfaces;

namespace SuperMarket.Domain.Entities.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IAuthenticatedUserService authenticatedUser;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
            IAuthenticatedUserService authenticatedUser)
            : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            this.authenticatedUser = authenticatedUser;
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableBaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.Created = DateTime.Now;
                        entry.Entity.CreatedBy = authenticatedUser.UserId;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModified = DateTime.Now;
                        entry.Entity.LastModifiedBy = authenticatedUser.UserId;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
