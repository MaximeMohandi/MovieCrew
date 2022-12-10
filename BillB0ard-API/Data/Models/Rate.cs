namespace BillB0ard_API.Data.Models
{
    public class Rate
    {
        public long UserId { get; set; }
        public int MovieId { get; set; }
        public decimal Note { get; set; }

        //Navigation Properties
        public Movie Movie { get; set; }
    }
}
