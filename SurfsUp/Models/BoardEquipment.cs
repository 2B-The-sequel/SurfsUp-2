namespace SurfsUp.Models
{
    public class BoardEquipment : IIdentifiable
    {
        public int Id { get; set; }

        public int BoardId { get; set; }
        public Board Board { get; set; }

        public int EquipmentId { get; set; }
        public Equipment Equipment { get; set; }
    }
}