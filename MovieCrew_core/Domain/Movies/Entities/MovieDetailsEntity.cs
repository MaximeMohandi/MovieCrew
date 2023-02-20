using MovieCrew_core.Domain.Users.Entities;

namespace MovieCrew_core.Domain.Movies.Entities
{
    public class MovieDetailsEntity : MovieEntity
    {
        public MovieDetailsEntity(int id,
                                  string title,
                                  string? poster,
                                  DateTime addedDate,
                                  DateTime? seenDate,
                                  decimal? averageRate,
                                  List<MovieRateEntity>? movieRates,
                                  UserEntity? proposedBy) :
            base(id, title, poster, addedDate, seenDate, averageRate)
        {
            MovieRates = movieRates;
            ProposedBy = proposedBy;
        }

        public List<MovieRateEntity>? MovieRates { get; }
        public MovieRateEntity? BestRate => MovieRates?.MaxBy(r => r.Rate);
        public MovieRateEntity? WorstRate => MovieRates?.MinBy(r => r.Rate);
        public UserEntity? ProposedBy { get; }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;

            var toCompare = (MovieDetailsEntity)obj;

            return base.Equals(obj)
                && RatesAreEquals(toCompare.MovieRates)
                && Equals(BestRate, toCompare.BestRate)
                && Equals(WorstRate, toCompare.WorstRate)
                && Equals(ProposedBy, toCompare.ProposedBy);
        }

        private bool RatesAreEquals(List<MovieRateEntity>? rates)
        {
            if (MovieRates is null || rates is null) return rates is null && MovieRates is null;

            return MovieRates.SequenceEqual(rates!);
        }

        public override int GetHashCode()
        {
            int hash = base.GetHashCode();
            if (MovieRates is not null)
            {
                foreach (MovieRateEntity rate in MovieRates)
                {
                    hash = HashCode.Combine(hash, EqualityComparer<MovieRateEntity>.Default.GetHashCode(rate), BestRate!.GetHashCode(), WorstRate!.GetHashCode());
                }
            }

            if (ProposedBy is not null)
            {
                hash = HashCode.Combine(hash, ProposedBy.GetHashCode());
            }

            return hash;
        }
    }
}
