using MovieCrew.Core.Domain.Users.Entities;

namespace MovieCrew.Core.Domain.Movies.Entities;

public class MovieDetailsEntity : MovieEntity
{
    public MovieDetailsEntity(int id,
        string title,
        string poster,
        string descsription,
        DateTime addedDate,
        DateTime? seenDate,
        decimal? averageRate,
        decimal? peopleAverageRate,
        decimal? revenue,
        decimal? budget,
        List<MovieRateEntity>? movieRates,
        UserEntity? proposedBy) :
        base(id, title, poster, descsription, addedDate, seenDate, averageRate)
    {
        PeopleAverageRate = peopleAverageRate;
        Revenue = revenue;
        Budget = budget;
        MovieRates = movieRates;
        ProposedBy = proposedBy;
    }

    public decimal? PeopleAverageRate { get; set; }
    public decimal? Revenue { get; set; }
    public decimal? Budget { get; set; }
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
               && Equals(PeopleAverageRate, toCompare.PeopleAverageRate)
               && Equals(Revenue, toCompare.Revenue)
               && Equals(Budget, toCompare.Budget)
               && Equals(ProposedBy, toCompare.ProposedBy);
    }

    private bool RatesAreEquals(List<MovieRateEntity>? rates)
    {
        if (MovieRates is null || rates is null) return rates is null && MovieRates is null;

        return MovieRates.SequenceEqual(rates!);
    }

    public override int GetHashCode()
    {
        var hash = base.GetHashCode();
        if (MovieRates is not null)
            foreach (var rate in MovieRates)
                hash = HashCode.Combine(hash, EqualityComparer<MovieRateEntity>.Default.GetHashCode(rate),
                    BestRate!.GetHashCode(), WorstRate!.GetHashCode());
        hash = HashCode.Combine(hash, ProposedBy is null ? 0 : ProposedBy.GetHashCode());
        hash = HashCode.Combine(hash, PeopleAverageRate is null ? 0 : PeopleAverageRate.GetHashCode());
        hash = HashCode.Combine(hash, Revenue is null ? 0 : Revenue.GetHashCode());
        hash = HashCode.Combine(hash, Budget is null ? 0 : Budget.GetHashCode());
        return hash;
    }
}
