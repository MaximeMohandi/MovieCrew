namespace BillB0ard_API.Data.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Poster { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime? SeenDate { get; set; } = null;

        //Navigation Properties
        public List<Rate>? Rates { get; set; }

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            Movie toCompare = (Movie)obj;

            return toCompare.Id == Id && toCompare.Name == Name
                && toCompare.Poster == Poster && toCompare.DateAdded.Equals(DateAdded)
                && toCompare.SeenDate.Equals(SeenDate) && toCompare.Rates == Rates;
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return GetHashCode();
        }
    }
}
