namespace BillB0ard_API.Domain.Entities
{
    public class RateEntity
    {
        public RateEntity(MovieEntity movieRated, UserEntity ratedBy, decimal rate)
        {
            this.MovieRated = movieRated;
            this.RatedBy = ratedBy;
            this.Rate = rate;
        }

        public MovieEntity MovieRated { get; }
        public UserEntity RatedBy { get; }
        public decimal Rate { get; }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            RateEntity toCompare = (RateEntity)obj;
            return MovieRated.Equals(toCompare.MovieRated) && RatedBy.Equals(toCompare.RatedBy)
                && Rate == toCompare.Rate;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(MovieRated, RatedBy, Rate);
        }
    }
}
