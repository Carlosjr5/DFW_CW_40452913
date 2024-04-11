namespace DFW_CW_40452913.Data
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime DatePosted { get; set; }

        // Foreign key
        public int PetitionId { get; set; }

        // Navigation property
        public Petition Petition { get; set; }
    }
}
