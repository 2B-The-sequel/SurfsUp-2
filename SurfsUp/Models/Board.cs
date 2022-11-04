using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using SurfsUp.Models.Validation;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SurfsUp.Models
{
    public class Board
    {
        [Key]
        public int BoardId { get; set; }
        [Required(ErrorMessage = "Der skal være et navn")]
        [DisplayName("Navn")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Der skal være et billede")]
        [DisplayName("Billede")]
        public string Image { get; set; }

        [Required(ErrorMessage = "Der skal være en længde")]
        [DisplayName("Længde")]
        [ValidOnlyPositive(ErrorMessage = "Længden skal være nul din taber")]
        public float Length { get; set; }
        [Required(ErrorMessage = "Der skal være en bredde")]
        [DisplayName("Bredde")]
        [ValidOnlyPositive(ErrorMessage = "Bredden skal være over nul din taber")]
        public float Width { get; set; }
        [Required(ErrorMessage = "Der skal være en tykkelse")]
        [DisplayName("Tykkelse")]
        [ValidOnlyPositive(ErrorMessage = "Tykkelsen skal være over nul din taber")]
        public float Thickness { get; set; }
        [Required(ErrorMessage = "Der skal være en volumen")]
        [DisplayName("Volumen")]
        [ValidOnlyPositive(ErrorMessage = "Volumen skal være over nul din taber")]
        public float Volume { get; set; }

        [Required(ErrorMessage = "Du tager fejl. Der skal være en pris.")]
        [DataType(DataType.Currency)]
        [DisplayName("Pris")]
        [ValidOnlyPositive(ErrorMessage = "Prisen skal være over nul din taber")]
        public decimal Price { get; set; }

        [JsonIgnore]
        [DisplayName("Udstyr")]
        public List<Equipment> Equipment { get; set; } = new List<Equipment>();

        [JsonConverter(typeof(StringEnumConverter))]
        [DisplayName("Type")]
        public BoardType Type { get; set; }

        [JsonIgnore]
        public ApplicationUser applicationUser { get; set; }

        [JsonIgnore]
        public ICollection<Rental> rentals { get ; set; }

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