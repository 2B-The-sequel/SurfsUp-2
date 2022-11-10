using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using SurfsUp.Data;
using System.Security.Claims;

namespace SurfsUp.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                bool changed = false;

                //Nyt start
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
                //Nyt Slut
                Board TheMinilog = new()
                {
                    Name = "The Minilog",
                    Image = "https://www.light-surfboards.com/uploads/5/7/3/0/57306051/s326152794241300969_p281_i54_w682.png?width=640",
                    Length = 6.0f,
                    Width = 21.0f,
                    Thickness = 2.75f,
                    Volume = 38.8f,
                    Type = BoardType.Shortboard,
                    Price = 565.0f
                };
                Board TheWideGlider = new()
                {
                    Name = "The Wide Glider",
                    Image = "https://www.light-surfboards.com/uploads/5/7/3/0/57306051/s326152794241300969_p281_i54_w682.png?width=640",
                    Length = 7.1f,
                    Width = 21.75f,
                    Thickness = 2.75f,
                    Volume = 44.16f,
                    Type = BoardType.Funboard,
                    Price = 685.0f
                };
                Board TheGoldenRatio = new()
                {
                    Name = "The Golden Ratio",
                    Image = "https://www.light-surfboards.com/uploads/5/7/3/0/57306051/s326152794241300969_p281_i54_w682.png?width=640",
                    Length = 6.3f,
                    Width = 21.85f,
                    Thickness = 2.9f,
                    Volume = 43.22f,
                    Type = BoardType.Funboard,
                    Price = 695.0f
                };
                Board MahiMahi = new()
                {
                    Name = "Mahi Mahi",
                    Image = "https://www.light-surfboards.com/uploads/5/7/3/0/57306051/s326152794241300969_p281_i54_w682.png?width=640",
                    Length = 5.4f,
                    Width = 20.75f,
                    Thickness = 2.3f,
                    Volume = 29.39f,
                    Type = BoardType.Fish,
                    Price = 645.0f
                };
                Board TheEmeraldGrinder = new()
                {
                    Name = "The Emerald Grinder",
                    Image = "https://www.light-surfboards.com/uploads/5/7/3/0/57306051/s326152794241300969_p281_i54_w682.png?width=640",
                    Length = 9.2f,
                    Width = 22.8f,
                    Thickness = 2.8f,
                    Volume = 65.4f,
                    Type = BoardType.Longboard,
                    Price = 895.0f
                };
                Board TheBomb = new()
                {
                    Name = "The Bomb",
                    Image = "https://www.light-surfboards.com/uploads/5/7/3/0/57306051/s326152794241300969_p281_i54_w682.png?width=640",
                    Length = 5.5f,
                    Width = 21.0f,
                    Thickness = 2.5f,
                    Volume = 33.7f,
                    Type = BoardType.Shortboard,
                    Price = 645.0f
                };
                Board WaldenMagic = new()
                {
                    Name = "Walden Magic",
                    Image = "https://www.light-surfboards.com/uploads/5/7/3/0/57306051/s326152794241300969_p281_i54_w682.png?width=640",
                    Length = 9.6f,
                    Width = 19.4f,
                    Thickness = 3.0f,
                    Volume = 80.0f,
                    Type = BoardType.Longboard,
                    Price = 1025.0f
                };
                Board NaishOne = new()
                {
                    Name = "Naish One",
                    Image = "https://www.light-surfboards.com/uploads/5/7/3/0/57306051/s326152794241300969_p281_i54_w682.png?width=640",
                    Length = 12.6f,
                    Width = 30.0f,
                    Thickness = 6.0f,
                    Volume = 301.0f,
                    Type = BoardType.SUP,
                    Price = 854.0f
                };
                Board SexTourer = new()
                {
                    Name = "Sex Tourer",
                    Image = "https://www.light-surfboards.com/uploads/5/7/3/0/57306051/s326152794241300969_p281_i54_w682.png?width=640",
                    Length = 11.6f,
                    Width = 32.0f,
                    Thickness = 6.0f,
                    Volume = 270.0f,
                    Type = BoardType.SUP,
                    Price = 611.0f
                };
                Board NaishMaliko = new()
                {
                    Name = "Naish Maliko",
                    Image = "https://www.light-surfboards.com/uploads/5/7/3/0/57306051/s326152794241300969_p281_i54_w682.png?width=640",
                    Length = 14.0f,
                    Width = 25.0f,
                    Thickness = 6.0f,
                    Volume = 330.0f,
                    Type = BoardType.SUP,
                    Price = 1304.0f
                };

                Equipment Paddle = new()
                {
                    Name = "Paddle"
                };
                Equipment Fin = new()
                {
                    Name = "Fin"
                };
                Equipment Pump = new()
                {
                    Name = "Pump"
                };
                Equipment Leash = new()
                {
                    Name = "Leash"
                };

                NaishOne.Equipment.Add(Paddle);
                SexTourer.Equipment.Add(Fin);
                SexTourer.Equipment.Add(Paddle);
                SexTourer.Equipment.Add(Pump);
                SexTourer.Equipment.Add(Leash);
                NaishMaliko.Equipment.Add(Fin);
                NaishMaliko.Equipment.Add(Paddle);
                NaishMaliko.Equipment.Add(Pump);
                NaishMaliko.Equipment.Add(Leash);

                Paddle.Boards.Add(NaishOne);
                Paddle.Boards.Add(SexTourer);
                Paddle.Boards.Add(NaishMaliko);
                Fin.Boards.Add(SexTourer);
                Fin.Boards.Add(NaishMaliko);
                Pump.Boards.Add(SexTourer);
                Pump.Boards.Add(NaishMaliko);
                Leash.Boards.Add(SexTourer);
                Leash.Boards.Add(NaishMaliko);

                if (!context.Board.Any())
                {
                    context.Board.AddRange(
                        TheMinilog,
                        TheWideGlider,
                        TheGoldenRatio,
                        MahiMahi,
                        TheEmeraldGrinder,
                        TheBomb,
                        WaldenMagic,
                        NaishOne,
                        SexTourer,
                        NaishMaliko
                    );

                    changed = true;
                }
                //Nyt Start
                if (!context.Users.Any())
                {
                    context.Users.AddRange(
                        GuestUser,
                        AdminUser
                        
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
                //Nyt Slut
                if (!context.Equipment.Any())
                {
                    context.Equipment.AddRange(
                        Paddle,
                        Fin,
                        Pump,
                        Leash
                    );

                    changed = true;
                }

                if (changed)
                    context.SaveChanges();
            }
        }
    }

}