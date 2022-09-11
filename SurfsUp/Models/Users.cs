using System.ComponentModel.DataAnnotations;

namespace SurfsUp.Models
{
    public class Users
    {
        [Key]
        public int UsersId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public ICollection<Rental> Rentals { get; set; }
    }
}
