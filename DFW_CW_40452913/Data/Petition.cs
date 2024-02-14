namespace DFW_CW_40452913.Data
{
    public class Petition
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public byte[] Image { get; set; } // Property for the image data
        public DateTime CreatedAt { get; set; }


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
                    Image = new byte[0], // Set the Image property to an empty byte array
                    CreatedAt = DateTime.Now,
                });
            }

            return petitions;
        }
    }


}
