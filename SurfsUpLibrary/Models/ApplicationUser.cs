using Microsoft.AspNetCore.Identity;

namespace SurfsUpLibrary.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Rental> rentals { get; set; }
    }
}