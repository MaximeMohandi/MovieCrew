namespace BillB0ard_API.Domain.Entities
{
    public class MovieEntity
    {
        public MovieEntity(int id, string title, string? poster, DateTime addedDate, DateTime? seenDate)
        {
            this.Id = id;
            this.Title = title;
            this.Poster = poster;
            this.AddedDate = addedDate;
            this.SeenDate = seenDate;
        }
        public int Id { get; }
        public string Title { get; }
        public string? Poster { get; }
        public DateTime AddedDate { get; }
        public DateTime? SeenDate { get; }
        public List<RateEntity>? Rates { get; set; } = null;
        public decimal? AverageRate => Rates?.Average(r => r.Rate);
        public decimal? LowestRates => Rates?.Min(r => r.Rate);
        public decimal? TopRate => Rates?.Max(r => r.Rate);

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            MovieEntity toCompare = (MovieEntity)obj;
            return Id.Equals(toCompare.Id) && Title == toCompare.Title
                && Poster == toCompare.Poster && AddedDate == toCompare.AddedDate
                && SeenDate == toCompare.SeenDate && RatesAreEquals(toCompare.Rates);
        }

        private bool RatesAreEquals(List<RateEntity>? rates)
        {
            if (Rates is null || rates is null) return rates is null && Rates is null;

            return Enumerable.SequenceEqual(Rates, rates!);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Title, AddedDate);
        }
    }
}
