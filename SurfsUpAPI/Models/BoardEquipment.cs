using System.ComponentModel.DataAnnotations;

namespace SurfsUpAPI.Models
{
    public class BoardEquipment
    {
        [Key]
        public int BoardEquipmentId { get; set; }
        public int BoardId { get; set; }
        public int EquipmentId { get; set; }
    }
}