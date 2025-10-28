namespace CookMaster_Project.Models
{
    public class User
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string SecurityQuestion { get; set; } = string.Empty;
        public string SecurityAnswer { get; set; } = string.Empty;

        //Allows for modification only during object creation.
        public bool IsAdmin { get; init; } = false;
    }
}

