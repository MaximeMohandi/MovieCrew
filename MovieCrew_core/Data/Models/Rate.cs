namespace MovieCrew_core.Data.Models
{
    public class Rate
    {
        public long UserId { get; set; }
        public int MovieId { get; set; }
        public decimal Note { get; set; }

        //Navigation Properties
        public Movie Movie { get; set; } = null!;
        public User User { get; set; } = null!;

        // override object.Equals
        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            Rate rate = (Rate)obj;

            return UserId == rate.UserId
                   && MovieId == rate.MovieId
                   && Note == rate.Note;
        }

        public override int GetHashCode()
        {
            return UserId.GetHashCode() + MovieId.GetHashCode();
        }
    }
}
