namespace SurfsUp.Models
{
    public class Equipment
    {
        public int EquipmentId { get; set; }
        public string Name { get; set; }
        public ICollection<Board> Boards { get; set; } = new List<Board>();
        public List<BoardEquipment> BoardEquipments { get; set; } = new List<BoardEquipment>();
    }
}