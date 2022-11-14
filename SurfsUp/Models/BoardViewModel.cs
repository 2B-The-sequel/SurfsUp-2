using System.ComponentModel;
using SurfsUpLibrary.Models;

namespace SurfsUp.Models
{
    public class BoardViewModel : Board
    {
        [DisplayName("Udstyr")]
        public List<EquipmentViewModel> EquipmentVM { get; set; }
    }
}