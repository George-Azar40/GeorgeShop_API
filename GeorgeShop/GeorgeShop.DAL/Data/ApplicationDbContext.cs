using GeorgeShop.DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GeorgeShop.DAL.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryTranslation> CategoryTranslations {  get; set; }

        private readonly IHttpContextAccessor _HttpContextAccessor;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
            IHttpContextAccessor HttpContextAccessor
            ) :base(options) 
        {
            _HttpContextAccessor = HttpContextAccessor;
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //fluent API
            builder.Entity<ApplicationUser>().ToTable("Users");
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            if(_HttpContextAccessor.HttpContext is not null)
            {
                var entries = ChangeTracker.Entries<AuditableEntity>();
                var currentUserId = _HttpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                foreach (var entry in entries)
                {
                    if (entry.State == EntityState.Added)
                    {
                        entry.Property(x => x.CreatedOn).CurrentValue = DateTime.UtcNow;
                        entry.Property(x => x.CreatedById).CurrentValue = currentUserId;
                    }

                    if (entry.State == EntityState.Modified)
                    {
                        entry.Property(x => x.UpdatedOn).CurrentValue = DateTime.UtcNow;
                        entry.Property(x => x.UpdatedById).CurrentValue = currentUserId;
                    }
                }
            }
            
            return base.SaveChangesAsync(cancellationToken);
        }

    }
}
