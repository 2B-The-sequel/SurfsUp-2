using Microsoft.AspNetCore.Identity;

namespace SurfsUp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Rental> rentals { get; set; }
    }
}
