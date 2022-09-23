using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using SurfsUp.Models.Validation;

namespace SurfsUp.Models
{
    public class Rental
    {
        [Key]
        public int RentalID { get; set; }

        [ForeignKey("UsersId")]
        public string UsersId { get; set; }
        [Required]
        [ForeignKey("BoardId")]
        public int BoardId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Startdato")]
        [ValidStartDate(ErrorMessage = "Start rentaldate must be greater than current date.") ]
        public DateTime StartRental { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Slutdato")]
        [ValidEndDate(ErrorMessage = "Start rentaldate must be greater than current date.")]
        public DateTime EndRental { get; set; }
      
        public IdentityUser User { get; set; }
        
        public Board Board { get; set; }

    }
}