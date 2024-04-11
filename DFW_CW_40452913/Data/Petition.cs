using System;
using System.ComponentModel.DataAnnotations;

namespace DFW_CW_40452913.Data
{
    public class Petition
    {
        public int Id { get; set; } // Primary key
        public required string Title { get; set; }
        public required string Description { get; set; }


        public  string? ImageUrl { get; set; }

        public List<Comment> Comments { get; set; } = new List<Comment>();

    }
}
