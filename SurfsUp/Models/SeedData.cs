using Microsoft.EntityFrameworkCore;
using SurfsUp.Data;

namespace SurfsUp.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                if (context.Board.Any())
                    return;

                context.Board.AddRange(
                
                    new Board
                    {
                        Name = "The Minilog",
                        Image = "https://www.light-surfboards.com/uploads/5/7/3/0/57306051/s326152794241300969_p281_i54_w682.png?width=640",
                        Length = 6.0f,
                        Width = 21.0f,
                        Thickness = 2.75f,
                        Volume = 38.8f,
                        Type = BoardType.Shortboard,
                        Price = 565.0f
                    },

                    new Board
                    {
                        Name = "The Wide Glider",
                        Image = "https://www.light-surfboards.com/uploads/5/7/3/0/57306051/s326152794241300969_p281_i54_w682.png?width=640",
                        Length = 7.1f,
                        Width = 21.75f,
                        Thickness = 2.75f,
                        Volume = 44.16f,
                        Type = BoardType.Funboard,
                        Price = 685.0f
                    },

                    new Board
                    {
                        Name = "The Golden Ratio",
                        Image = "https://www.light-surfboards.com/uploads/5/7/3/0/57306051/s326152794241300969_p281_i54_w682.png?width=640",
                        Length = 6.3f,
                        Width = 21.85f,
                        Thickness = 2.9f,
                        Volume = 43.22f,
                        Type = BoardType.Funboard,
                        Price = 695.0f
                    },

                    new Board
                    {
                        Name = "Mahi Mahi",
                        Image = "https://www.light-surfboards.com/uploads/5/7/3/0/57306051/s326152794241300969_p281_i54_w682.png?width=640",
                        Length = 5.4f,
                        Width = 20.75f,
                        Thickness = 2.3f,
                        Volume = 29.39f,
                        Type = BoardType.Fish,
                        Price = 645.0f
                    },

                    new Board
                    {
                        Name = "The Emerald Grinder",
                        Image = "https://www.light-surfboards.com/uploads/5/7/3/0/57306051/s326152794241300969_p281_i54_w682.png?width=640",
                        Length = 9.2f,
                        Width = 22.8f,
                        Thickness = 2.8f,
                        Volume = 65.4f,
                        Type = BoardType.Longboard,
                        Price = 895.0f
                    },

                    new Board
                    {
                        Name = "The Bomb",
                        Image = "https://www.light-surfboards.com/uploads/5/7/3/0/57306051/s326152794241300969_p281_i54_w682.png?width=640",
                        Length = 5.5f,
                        Width = 21.0f,
                        Thickness = 2.5f,
                        Volume = 33.7f,
                        Type = BoardType.Shortboard,
                        Price = 645.0f
                    },

                    new Board
                    {
                        Name = "Walden Magic",
                        Image = "https://www.light-surfboards.com/uploads/5/7/3/0/57306051/s326152794241300969_p281_i54_w682.png?width=640",
                        Length = 9.6f,
                        Width = 19.4f,
                        Thickness = 3.0f,
                        Volume = 80.0f,
                        Type = BoardType.Longboard,
                        Price = 1025.0f
                    },

                    new Board
                    {
                        Name = "Naish One",
                        Image = "https://www.light-surfboards.com/uploads/5/7/3/0/57306051/s326152794241300969_p281_i54_w682.png?width=640",
                        Length = 12.6f,
                        Width = 30.0f,
                        Thickness = 6.0f,
                        Volume = 301.0f,
                        Type = BoardType.SUP,
                        Price = 854.0f
                    },

                    new Board
                    {
                        Name = "Sex Tourer",
                        Image = "https://www.light-surfboards.com/uploads/5/7/3/0/57306051/s326152794241300969_p281_i54_w682.png?width=640",
                        Length = 11.6f,
                        Width = 32.0f,
                        Thickness = 6.0f,
                        Volume = 270.0f,
                        Type = BoardType.SUP,
                        Price = 611.0f
                    },

                    new Board
                    {
                        Name = "Naish Maliko",
                        Image = "https://www.light-surfboards.com/uploads/5/7/3/0/57306051/s326152794241300969_p281_i54_w682.png?width=640",
                        Length = 14.0f,
                        Width = 25.0f,
                        Thickness = 6.0f,
                        Volume = 330.0f,
                        Type = BoardType.SUP,
                        Price = 1304.0f
                    }
                );

                context.SaveChanges();
            }
        }
    }
}