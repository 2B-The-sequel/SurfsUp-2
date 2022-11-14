namespace SurfsUpLibrary.Models
{
    public class Lock
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }

        public Lock(int Id, DateTime Time)
        {
            this.Id = Id;
            this.Time = Time;
        }
    }
}