using System;
using System.Collections.Generic;

namespace DFW_CW_40452913.Data
{
    public class Petition
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; } // Change type to string for image URL
        public DateTime DateCreated { get; set; } // Change property name to match sample data
        public string Author { get; set; }
        public int Votes { get; set; }


        public static List<Petition> GenerateRandomPetitions(int count)
        {
            var petitions = new List<Petition>();

            for (int i = 1; i <= count; i++)
            {
                petitions.Add(new Petition
                {
                    Id = i, // Set the Id property to a unique value for each petition
                    Title = $"Petition Title {i}",
                    Description = $"Description of Petition {i}",
                    ImageUrl = "", // Set the ImageUrl property to an empty string or provide a default URL
                    DateCreated = DateTime.Now, // Set the DateCreated property
                });
            }

            return petitions;
        }
    }
}
