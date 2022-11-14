using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using SurfsUpLibrary.Models.Validation;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations.Schema;

namespace SurfsUpLibrary.Models
{
    public class Board : IIdentifiable
    {
        [Key]
        public int Id { get; set; }
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

        [JsonIgnore, NotMapped]
        [DisplayName("Udstyr")]
        public List<Equipment> Equipment { get; set; } = new List<Equipment>();

        [JsonConverter(typeof(StringEnumConverter))]
        [DisplayName("Type")]
        public BoardType Type { get; set; }

        [JsonIgnore]
        public ApplicationUser applicationUser { get; set; }

        [JsonIgnore, NotMapped]
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