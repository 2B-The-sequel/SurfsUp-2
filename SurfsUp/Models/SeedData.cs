using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using SurfsUp.Data;
using System.Security.Claims;

namespace SurfsUp.Models
{
    public class Seeddata
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                bool changed = false;


                IdentityUser AdminUser = new()
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
                IdentityUser GuestUser = new()
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
                
                
                if (!context.Users.Any())
                {
                    context.Users.AddRange(
                        (ApplicationUser)GuestUser, //Den skulle ikke typecastes før???
                        (ApplicationUser)AdminUser
                        );
                    changed = true;
                }
                if (!context.Roles.Any())
                {
                    context.Roles.AddRange(
                        AdminRole,
                        UserRole
                        );
                    changed = true;
                }
                if (!context.UserRoles.Any())
                {
                    context.UserRoles.AddRange(
                        AdminToRole
                        );
                    changed = true;
                }


                if (changed)
                    context.SaveChanges();
            }
        }
    }
}
