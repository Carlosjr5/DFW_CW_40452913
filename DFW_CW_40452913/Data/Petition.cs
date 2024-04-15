using System;
using System.Collections.Generic; // Add this for List<>
using System.ComponentModel.DataAnnotations;

namespace DFW_CW_40452913.Data
{
    public class Petition
    {
        public int Id { get; set; } // Primary key

        // Use the Required attribute instead of required
        [Required]
        public string Title { get; set; }

        // Use the Required attribute instead of required
        [Required]
        public string Description { get; set; }

        // Use the ? operator to denote nullable reference type
        public string? ImageUrl { get; set; }

        // Add using System.Collections.Generic; for List<>
        public List<Comment> Comments { get; set; } = new List<Comment>();
        public int Votes { get; set; } = 0;
    }
}
