using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using SurfsUpLibrary.Models;

namespace SurfsUpAPI.Data
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using ApplicationDbContext context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());
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
                Price = 565
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
                Price = 685
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
                Price = 695
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
                Price = 645
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
                Price = 895
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
                Price = 645
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
                Price = 1025
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
                Price = 854
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
                Price = 611
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
                Price = 1304
            };

            if (!context.Set<Board>().Any())
            {
                context.Set<Board>().AddRange(
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

            if (changed)
                context.SaveChanges();

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

            if (!context.Set<Equipment>().Any())
            {
                context.Set<Equipment>().AddRange(
                    Paddle,
                    Fin,
                    Pump,
                    Leash
                );

                changed = true;
            }

            if (changed)
                context.SaveChanges();

            BoardEquipment be1 = new()
            {
                BoardId = NaishOne.Id,
                EquipmentId = Paddle.Id
            };
            BoardEquipment be2 = new()
            {
                BoardId = SexTourer.Id,
                EquipmentId = Paddle.Id
            };
            BoardEquipment be3 = new()
            {
                BoardId = SexTourer.Id,
                EquipmentId = Fin.Id
            };
            BoardEquipment be4 = new()
            {
                BoardId = SexTourer.Id,
                EquipmentId = Pump.Id
            };
            BoardEquipment be5 = new()
            {
                BoardId = SexTourer.Id,
                EquipmentId = Leash.Id
            };
            BoardEquipment be6 = new()
            {
                BoardId = NaishMaliko.Id,
                EquipmentId = Paddle.Id
            };
            BoardEquipment be7 = new()
            {
                BoardId = NaishMaliko.Id,
                EquipmentId = Fin.Id
            };
            BoardEquipment be8 = new()
            {
                BoardId = NaishMaliko.Id,
                EquipmentId = Pump.Id
            };
            BoardEquipment be9 = new()
            {
                BoardId = NaishMaliko.Id,
                EquipmentId = Leash.Id
            };

            if (!context.Set<BoardEquipment>().Any())
            {
                context.Set<BoardEquipment>().AddRange(
                    be1,
                    be2,
                    be3,
                    be4,
                    be5,
                    be6,
                    be7,
                    be8,
                    be9
                );

                changed = true;
            }

            if (changed)
                context.SaveChanges();
        }
    }
}