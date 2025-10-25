using System;

namespace CookMaster_Project.Models
{
    public class Recipe
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string Type { get; set; } = string.Empty; // Type of cuisine
    }
}