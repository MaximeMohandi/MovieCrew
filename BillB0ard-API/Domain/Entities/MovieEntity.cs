namespace BillB0ard_API.Domain.Entities
{
    public record MovieEntity(int Id, string Title, string? Poster, DateTime AddedDate, DateTime? SeenDate)
    {
        public List<RateEntity>? Rates { get; set; } = null;
        public decimal? AverageRate => Rates?.Average(r => r.Rate);

        public decimal? LowestRates => Rates?.Min(r => r.Rate);

        public decimal? TopRate => Rates?.Max(r => r.Rate);
    }
}
