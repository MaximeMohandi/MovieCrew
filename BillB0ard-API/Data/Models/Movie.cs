namespace BillB0ard_API.Data.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Poster { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime? SeenDate { get; set; }

        //Navigation Properties
        public List<Rate>? Rates { get; set; }
    }
}
