namespace BillB0ard_API.Domain.Entities
{
    public class MovieDetailsEntity : MovieEntity
    {
        public MovieDetailsEntity(int id,
                                  string title,
                                  string? poster,
                                  DateTime addedDate,
                                  DateTime? seenDate,
                                  decimal? averageRate,
                                  List<MovieRateEntity>? movieRates) :
            base(id, title, poster, addedDate, seenDate, averageRate)
        {
            MovieRates = movieRates;
        }

        public List<MovieRateEntity>? MovieRates { get; }
        public MovieRateEntity? BestRate => MovieRates?.MaxBy(r => r.Rate);
        public MovieRateEntity? WorstRate => MovieRates?.MinBy(r => r.Rate);

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;

            var toCompare = (MovieDetailsEntity)obj;

            return base.Equals(obj)
                && RatesAreEquals(toCompare.MovieRates)
                && Equals(BestRate, toCompare.BestRate)
                && Equals(WorstRate, toCompare.WorstRate);
        }

        private bool RatesAreEquals(List<MovieRateEntity>? rates)
        {
            if (MovieRates is null || rates is null) return rates is null && MovieRates is null;

            return Enumerable.SequenceEqual(MovieRates, rates!);
        }

        public override int GetHashCode()
        {
            int hash = base.GetHashCode();
            if (MovieRates is null)
            {
                return HashCode.Combine(hash, 0);
            }

            foreach (MovieRateEntity rate in MovieRates)
            {
                hash = HashCode.Combine(hash, EqualityComparer<MovieRateEntity>.Default.GetHashCode(rate));
            }

            hash = HashCode.Combine(hash, BestRate!.GetHashCode(), WorstRate!.GetHashCode());

            return hash;
        }
    }
}
