using BillB0ard_API.Domain.Users.Entities;

namespace BillB0ard_API.Domain.Ratings.Entities
{
    public class RateEntity
    {
        public RateEntity(UserEntity ratedBy, decimal rate)
        {
            RatedBy = ratedBy;
            Rate = rate;
        }

        public UserEntity RatedBy { get; }
        public decimal Rate { get; }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            RateEntity toCompare = (RateEntity)obj;

            return RatedBy.Equals(toCompare.RatedBy)
                && Rate == toCompare.Rate;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(RatedBy, Rate);
        }
    }
}
