using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SurfsUpLibrary.Models
{
    public class BoardEquipment : IIdentifiable
    {
        [Key]
        public int Id { get; set; }

        public int BoardId { get; set; }
        [JsonIgnore]
        [NotMapped]
        public Board Board { get; set; }

        public int EquipmentId { get; set; }
        [JsonIgnore]
        [NotMapped]
        public Equipment Equipment { get; set; }
    }
}