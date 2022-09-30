namespace SurfsUpAPI.Models
{
    public class Users
    {
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public string? NormalizedUserName { get; set; }
        public string? Email { get; set; }
        public string? NormalizedEmail { get; set; }
        public bool? EmailConfirmed { get; set; }
        public string? PasswordHash { get; set; }
        public string? SecurityStamp { get; set; }
        public string? ConcurrencyStamp { get; set; }
        public string? Phonenumber { get; set; }
        public bool? PhonenumberConfirmed { get; set; }
        public bool? TwofactorEnabled { get; set; }
        public DateTime? LockedOutEnd { get; set; }
        public bool? LockedOutEnabled { get; set; }
        public int? AccedFailedCount { get; set; }

    }
}
