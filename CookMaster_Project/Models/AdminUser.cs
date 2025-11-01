using System;
using System.Collections.Generic;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CookMaster_Project.Models
{
    // AdminUser inherits from User and adds admin-specific functionality
    public class AdminUser : User
    {
        public AdminUser()
        {
            IsAdmin = true; // Ensure IsAdmin is always true for AdminUser
        }
    }
}