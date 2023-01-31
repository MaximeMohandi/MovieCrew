namespace BillB0ard_API.Domain.Entities
{
    public class MovieEntity
    {
        public MovieEntity(int id, string title, string? poster, DateTime dateAdded, DateTime? viewingDate)
        {
            this.Id = id;
            this.Title = title;
            this.Poster = poster;
            this.DateAdded = dateAdded;
            this.ViewingDate = viewingDate;
        }

        public MovieEntity(int id, string title, string? poster, DateTime addedDate, DateTime? seenDate, decimal? averageRate)
        {
            this.Id = id;
            this.Title = title;
            this.Poster = poster;
            this.DateAdded = addedDate;
            this.ViewingDate = seenDate;
            this.AverageRate = averageRate;
        }

        public int Id { get; }
        public string Title { get; }
        public string? Poster { get; }
        public DateTime DateAdded { get; }
        public DateTime? ViewingDate { get; }
        public decimal? AverageRate { get; } = null;

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;

            MovieEntity toCompare = (MovieEntity)obj;


            return Id.Equals(toCompare.Id) && Title == toCompare.Title
                && Poster == toCompare.Poster && DateAdded == toCompare.DateAdded
                && ViewingDate == toCompare.ViewingDate && AverageRate == toCompare.AverageRate;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Title, DateAdded) + HashcodesNullablePropoerties();
        }

        private int HashcodesNullablePropoerties()
        {
            return (Poster is null ? 0 : Poster.GetHashCode())
                + (ViewingDate is null ? 0 : ViewingDate.GetHashCode())
                + (AverageRate is null ? 0 : AverageRate.GetHashCode());
        }
    }
}
