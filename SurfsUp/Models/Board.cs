using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SurfsUp.Models.Validation;

namespace SurfsUp.Models
{
    public class Board
    {
        [Key]
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

        [DisplayName("Udstyr")]
        public List<Equipment> Equipment { get; set; } = new List<Equipment>();
        public List<BoardEquipment> BoardEquipments { get; set; } = new List<BoardEquipment>();

        [JsonConverter(typeof(StringEnumConverter))]
        [DisplayName("Type")]
        public BoardType Type { get; set; }

        public ApplicationUser applicationUser { get; set; }

        public ICollection<Rental> rentals { get; set; }

        public bool IsRented()
        {
            if (rentals == null)
            {
                return false;
            }
            foreach (Rental rental in rentals)
            {
                if (rental.EndRental > DateTime.Now && rental.StartRental <= DateTime.Now || rental.StartRental.Day == DateTime.Now.Day)
                { return true; }

               
            }
            return false;

        }


    }
}