using System;
using System.ComponentModel.DataAnnotations;

namespace DFW_CW_40452913.Data
{
    public class Petition
    {
        public int Id { get; set; } // Primary key
        public string Title { get; set; }
        public string Description { get; set; }


        public  string? ImageUrl { get; set; }

    

    }
}
