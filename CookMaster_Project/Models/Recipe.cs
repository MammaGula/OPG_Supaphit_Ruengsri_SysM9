using System;

namespace CookMaster_Project.Models
{
    public class Recipe
    {
        //Fields for basic recipe info
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // Fields for recipe details
        public string Ingredients { get; set; } = string.Empty;
        public string Instructions { get; set; } = string.Empty;
        public int TimeMinutes { get; set; } = 0;                 // Total cooking time in minutes

        // Type of cuisine
        public string Type { get; set; } = string.Empty;


        // Fields for tracking recipe metadata
        public string CreatedBy { get; set; } = string.Empty;     // Creator's username
        public DateTime CreatedDate { get; set; } = DateTime.Now; // Creation date
    }
}