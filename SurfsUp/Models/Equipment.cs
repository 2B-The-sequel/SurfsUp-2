using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SurfsUp.Models
{
    public class Equipment
    {
        public int EquipmentId { get; set; }
        [DisplayName("Navn")]
        [Required(ErrorMessage = "Udstyret skal have et navn")]
        public string Name { get; set; }
        public ICollection<Board> Boards { get; set; } = new List<Board>();
        public List<BoardEquipment> BoardEquipments { get; set; } = new List<BoardEquipment>();
    }
}