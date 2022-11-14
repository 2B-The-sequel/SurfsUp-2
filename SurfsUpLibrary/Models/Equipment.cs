using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SurfsUpLibrary.Models
{
    public class Equipment : IIdentifiable
    {
        public int Id { get; set; }
        [DisplayName("Navn")]
        [Required(ErrorMessage = "Udstyret skal have et navn")]
        public string Name { get; set; }
        [JsonIgnore]
        public List<Board> Boards { get; set; } = new List<Board>();
    }
}