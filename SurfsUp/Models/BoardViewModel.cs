using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SurfsUp.Models
{
    public class BoardViewModel
    {
        public int BoardId { get; set; }
        [DisplayName("Navn")]
        public string Name { get; set; }
        [DisplayName("Billede")]
        public string Image { get; set; }

        [DisplayName("Længde")]
        public float Length { get; set; }
        [DisplayName("Bredde")]
        public float Width { get; set; }
        [DisplayName("Tykkelse")]
        public float Thickness { get; set; }
        [DisplayName("Volumen")]
        public float Volume { get; set; }

        [Required(ErrorMessage = "Du tager fejl. Der skal være en pris.")]
        [DataType(DataType.Currency)]
        [DisplayName("Pris")]
        public float Price { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        [DisplayName("Type")]
        public BoardType Type { get; set; }

        [DisplayName("Udstyr")]
        public List<EquipmentViewModel> Equipment { get; set; }
    }
}