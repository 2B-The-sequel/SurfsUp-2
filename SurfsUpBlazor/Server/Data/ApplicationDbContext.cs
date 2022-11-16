using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SurfsUpBlazor.Server.Models;

namespace SurfsUpBlazor.Server.Data
{
	public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
	{
		public ApplicationDbContext(DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
		{
            bool changed = false;

            ApplicationUser AdminUser = new()
            {
                UserName = "Admin",
                Email = "admin@admin.admin",
                Id = "=0dc76dc5-d957-4aa6-8d1d-85301ac377ab",
                AccessFailedCount = 0,
                EmailConfirmed = true,
                ConcurrencyStamp = "",
                LockoutEnabled = false,
                NormalizedEmail = "ADMIN@ADMIN.ADMIN",
                PhoneNumber = null,
                TwoFactorEnabled = false,
                LockoutEnd = null,
                NormalizedUserName = "ADMIN@ADMIN.ADMIN",
                PasswordHash = "AQAAAAEAACcQAAAAEGDUXbGyj5Y26UJd97uFU/lwRUV+ETqPe2IpgliwoE9Afvs1vD19YO+eWYznFNeLjQ==",
                SecurityStamp = Guid.NewGuid().ToString("N"),
                PhoneNumberConfirmed = false
            };
            ApplicationUser GuestUser = new()
            {
                UserName = "Guest",
                Email = "Guest@Guest.com",
                Id = "1",
                AccessFailedCount = 0,
                EmailConfirmed = true,
                ConcurrencyStamp = "",
                LockoutEnabled = false,
                NormalizedEmail = "Guest@Guest.com",
                PhoneNumber = "11111111",
                TwoFactorEnabled = false,
                LockoutEnd = null,
                NormalizedUserName = "Guest",
                PasswordHash = "",
                SecurityStamp = Guid.NewGuid().ToString("D"),
                PhoneNumberConfirmed = true
            };
            IdentityRole AdminRole = new()
            {
                Id = "1",
                Name = "Administrators",
                NormalizedName = "Admin"
            };
            IdentityRole UserRole = new()
            {
                Id = "2",
                Name = "Users",
                NormalizedName = "User"
            };
            IdentityUserRole<string> AdminToRole = new()
            {
                RoleId = AdminRole.Id,
                UserId = AdminUser.Id
            };

            if (!Users.Any())
            {
                Users.AddRange(GuestUser, AdminUser);
                changed = true;
            }
            if (!Roles.Any())
            {
                Roles.AddRange(AdminRole, UserRole);
                changed = true;
            }

            if (!UserRoles.Any())
            {
                UserRoles.AddRange(AdminToRole);
                changed = true;
            }

            if (changed)
                SaveChanges();
        }
    }
}