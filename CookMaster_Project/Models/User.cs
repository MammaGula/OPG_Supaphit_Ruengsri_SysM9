namespace CookMaster_Project.Models
{
    public class User
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string SecurityQuestion { get; set; } = string.Empty;
        public string SecurityAnswer { get; set; } = string.Empty;
       
        // Admin rights: Admin can see/delete everyone's recipes.
        public bool IsAdmin { get; set; } = false;
    }

}
