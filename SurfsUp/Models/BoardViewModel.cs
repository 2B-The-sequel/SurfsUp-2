using System.ComponentModel;

namespace SurfsUp.Models
{
    public class BoardViewModel : Board
    {
        [DisplayName("Udstyr")]
        public List<EquipmentViewModel> EquipmentVM { get; set; }
    }
}