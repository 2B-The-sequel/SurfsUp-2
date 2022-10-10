using System.ComponentModel.DataAnnotations;

namespace SurfsUpAPI.Models
{
    public class Board
    {
        [Key]
        public int BoardId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }

        public float Length { get; set; }
        public float Width { get; set; }
        public float Thickness { get; set; }
        public float Volume { get; set; }

        public decimal Price { get; set; }
        public BoardType Type { get; set; }
    }
}