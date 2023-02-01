namespace BillB0ard_API.Domain.Entities
{
    public class MovieRateEntity
    {
        public MovieRateEntity(UserEntity ratedBy, decimal rate)
        {
            RatedBy = ratedBy;
            Rate = rate;
        }

        public UserEntity RatedBy { get; }
        public decimal Rate { get; }

        public override bool Equals(object? obj)
        {

            if (obj == null) return false;
            var toCompare = (MovieRateEntity)obj;
            return Equals(RatedBy, toCompare.RatedBy) && Rate == toCompare.Rate;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(RatedBy, Rate);
        }
    }
}
