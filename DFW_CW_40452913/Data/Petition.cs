using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DFW_CW_40452913.Data
{
    public class Petition
    {
        public int Id { get; set; } 

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public string? ImageUrl { get; set; }

        public List<Comment> Comments { get; set; } = new List<Comment>();
        public int Votes { get; set; } = 0;
    }
}
