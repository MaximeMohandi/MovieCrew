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

        public MovieEntity(int id, string title, string? poster, DateTime addedDate, DateTime? seenDate, decimal? averageRate)
        {
            this.Id = id;
            this.Title = title;
            this.Poster = poster;
            this.AddedDate = addedDate;
            this.SeenDate = seenDate;
            this.AverageRate = averageRate;
        }

        public int Id { get; }
        public string Title { get; }
        public string? Poster { get; }
        public DateTime AddedDate { get; }
        public DateTime? SeenDate { get; }
        public decimal? AverageRate { get; } = null;

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;

            MovieEntity toCompare = (MovieEntity)obj;


            return Id.Equals(toCompare.Id) && Title == toCompare.Title
                && Poster == toCompare.Poster && AddedDate == toCompare.AddedDate
                && SeenDate == toCompare.SeenDate && AverageRate == toCompare.AverageRate;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Title, AddedDate) + HashcodesNullablePropoerties();
        }

        private int HashcodesNullablePropoerties()
        {
            return (Poster is null ? 0 : Poster.GetHashCode())
                + (SeenDate is null ? 0 : SeenDate.GetHashCode())
                + (AverageRate is null ? 0 : AverageRate.GetHashCode());
        }
    }
}
