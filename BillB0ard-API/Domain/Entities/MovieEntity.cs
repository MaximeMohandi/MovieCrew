namespace BillB0ard_API.Domain.Entities
{
    public record MovieEntity(int Id, string Title, string? Poster, DateTime AddedDate, DateTime? SeenDate)
    {
        public List<RateEntity>? Rates { get; set; } = null;
        public decimal? AverageRate
        {
            get
            {
                if (Rates is null)
                {
                    return null;
                }
                return Rates.Average(r => r.Rate);
            }
        }

        public decimal? lowestRates
        {
            get
            {
                if (Rates is null) { return null; }
                return Rates.Min(r => r.Rate);
            }
        }
    }
}
