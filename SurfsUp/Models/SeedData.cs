using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using SurfsUp.Data;

namespace SurfsUp.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                bool changed = false;

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