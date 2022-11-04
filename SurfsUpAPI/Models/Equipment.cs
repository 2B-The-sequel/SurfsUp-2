using System.ComponentModel.DataAnnotations;

namespace SurfsUpAPI.Models
{
    public class Equipment : IIdentifiable
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}