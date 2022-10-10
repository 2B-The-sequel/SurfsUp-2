using System.ComponentModel.DataAnnotations;

namespace SurfsUpAPI.Models
{
    public class Equipment
    {
        [Key]
        public int EquipmentId { get; set; }
        public string Name { get; set; }
    }
}