using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using SurfsUpLibrary.Models.Validation;

namespace SurfsUpLibrary.Models
{
    public class Rental : IIdentifiable
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("UsersId")]
        public string UsersId { get; set; }
        [Required]
        [ForeignKey("BoardId")]
        public int BoardId { get; set; }

        [DisplayName("Fulde navn")]
        public string GuestName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Startdato")]
        [ValidStartDate]
        public DateTime StartRental { get; set; }

        [Required]
        [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Slutdato")]
        [ValidEndDate]
        public DateTime EndRental { get; set; }

        [JsonIgnore, NotMapped]
        public Board Board { get; set; }
    }
}