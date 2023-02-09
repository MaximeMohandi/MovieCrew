namespace BillB0ard_API.Domain.Users.Entities
{
    public class SpectatorDetailsEntity
    {
        public SpectatorDetailsEntity(UserEntity spectator, List<SpectatorRateEntity>? rates)
        {
            Spectator = spectator;
            Rates = rates;
        }
        public UserEntity Spectator { get; }
        public List<SpectatorRateEntity>? Rates { get; }
        public SpectatorRateEntity? BestRate => Rates?.MaxBy(r => r.Rate);
        public SpectatorRateEntity? WorstRate => Rates?.MinBy(r => r.Rate);

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;

            var toCompare = (SpectatorDetailsEntity)obj;

            return Equals(Spectator, toCompare.Spectator)
                && RatesAreEquals(toCompare.Rates)
                && Equals(BestRate, toCompare.BestRate)
                && Equals(WorstRate, toCompare.WorstRate);
        }

        private bool RatesAreEquals(List<SpectatorRateEntity>? rates)
        {
            if (Rates is null || rates is null) return rates is null && Rates is null;

            return Rates.SequenceEqual(rates!);
        }

        public override int GetHashCode()
        {
            int hash = 0;
            if (Rates is null) return hash;

            foreach (SpectatorRateEntity rate in Rates)
            {
                hash = HashCode.Combine(hash, rate);
            }

            hash = HashCode.Combine(hash, BestRate!.GetHashCode(), WorstRate!.GetHashCode());

            return hash;
        }
    }
}
