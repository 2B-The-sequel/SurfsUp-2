using System.ComponentModel.DataAnnotations;

namespace SurfsUpAPI.Models
{
    public class BoardEquipment : IIdentifiable
    {
        [Key]
        public int Id { get; set; }
        public int BoardId { get; set; }
        public int EquipmentId { get; set; }
    }
}