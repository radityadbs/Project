namespace FinalProject.Models
{
    public class UserLog
    {
        public int Id { get; set; }

        public string Email { get; set; } = "";
        public string Action { get; set; } = "";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(7);
    }
}
