namespace FinalProject.Models
{
    public class User
    {
        public int Id { get; set; }

        // ================= AUTH =================
        public string Email { get; set; } = "";
        public string Username { get; set; } = "";
        public string PasswordHash { get; set; } = "";
        public bool IsEmailVerified { get; set; }
        public DateTime? EmailVerifiedAt { get; set; }

        // ================= PROFILE =================
        public string Nik { get; set; } = "";             
        public string FullName { get; set; } = "";         
        public string Address { get; set; } = "";          
        public string PhoneNumber { get; set; } = "";      
        public string DesaId { get; set; } = "";    
        public string DesaName { get; set; } = "";       
        public string? PhotoPath { get; set; }            
    }
}
