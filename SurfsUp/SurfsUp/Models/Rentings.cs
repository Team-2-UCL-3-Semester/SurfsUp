namespace SurfsUp.Models
{
    public class Rentings
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid UserId { get; set; }
        public Guid BoardId { get; set; }

    }
}
