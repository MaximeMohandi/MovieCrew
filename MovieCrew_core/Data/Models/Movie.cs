namespace MovieCrew.Core.Data.Models;

public class Movie
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Poster { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime DateAdded { get; set; }
    public DateTime? SeenDate { get; set; } = null;
    public long? ProposedById { get; set; } = null;

    //Navigation Properties
    public List<Rate>? Rates { get; set; }
    public User? ProposedBy { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj == null) return false;

        var toCompare = (Movie)obj;

        return toCompare.Id == Id && toCompare.Name == Name
                                  && toCompare.Poster == Poster && toCompare.DateAdded.Equals(DateAdded)
                                  && toCompare.Description == Description
                                  && toCompare.SeenDate.Equals(SeenDate) && RatesAreEquals(toCompare.Rates);
    }

    private bool RatesAreEquals(List<Rate>? rates)
    {
        if (Rates is null || rates is null) return Rates is null && rates is null;

        return Rates.SequenceEqual(rates);
    }

    // override object.GetHashCode
    public override int GetHashCode()
    {
        var hash = HashCode.Combine(Id, Name, DateAdded);
        hash = HashCode.Combine(hash, Poster is null ? 0 : Poster.GetHashCode());
        hash = HashCode.Combine(hash, SeenDate is null ? 0 : SeenDate.GetHashCode());
        hash = HashCode.Combine(hash, ProposedById is null ? 0 : ProposedById.GetHashCode());
        hash = HashCode.Combine(hash, Description is null ? 0 : Description.GetHashCode());
        return hash;
    }
}