namespace MovieCrew_core.Data.Models
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

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;

            Movie toCompare = (Movie)obj;

            return toCompare.Id == Id && toCompare.Name == Name
                && toCompare.Poster == Poster && toCompare.DateAdded.Equals(DateAdded)
                && toCompare.SeenDate.Equals(SeenDate) && RatesAreEquals(toCompare.Rates);
        }

        private bool RatesAreEquals(List<Rate>? rates)
        {
            if (Rates is null || rates is null)
            {
                return Rates is null && rates is null;
            }

            return Enumerable.SequenceEqual(Rates, rates);
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, DateAdded);
        }
    }
}
