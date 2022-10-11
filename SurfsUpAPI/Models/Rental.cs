using SurfsUp.Models.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System;

namespace SurfsUpAPI.Models
{
    public class Rental : IIdentifiable
    {
        [Key]
        public int Id { get; set; }   //RentalID

        [ForeignKey("UsersId")]
        public string UsersId { get; set; }
        [Required]
        [ForeignKey("BoardId")]
        public int BoardId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Startdato")]

        public DateTime StartRental { get; set; }

        [Required]
        [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Slutdato")]

        public DateTime EndRental { get; set; }
    }
}
