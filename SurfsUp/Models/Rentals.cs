using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SurfsUp.Models
{
    public class Rental
    {
        [Key]
        public int RentalID { get; set; }

        [Required]
        [ForeignKey("UsersId")]
        public int UsersId { get; set; }
        [Required]
        [ForeignKey("BoardId")]
        public int BoardId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Startdato")]
        public DateTime StartRental { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Slutdato")]
        public DateTime EndRental { get; set; }
      

        public Users User { get; set; }
        
        public Board Board { get; set; }

    }
}