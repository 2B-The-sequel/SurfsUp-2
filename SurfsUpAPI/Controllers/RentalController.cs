using SurfsUpAPI.Data;
using SurfsUpLibrary.Models;

namespace SurfsUpAPI.Controllers
{
    public class RentalController : GenericAPIController<Rental>
    {
        public RentalController(ApplicationDbContext context) : base(context) { }
    }
}