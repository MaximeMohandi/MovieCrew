namespace BillB0ard_API.Data.Models
{
    public class Rate
    {
        public long UserId { get; set; }
        public int MovieId { get; set; }
        public decimal Note { get; set; }

        //Navigation Properties
        public Movie Movie { get; set; }
        public User User { get; set; }

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            Rate rate = (Rate)obj;

            return UserId == rate.UserId
                   && MovieId == rate.MovieId
                   && Note == rate.Note;
        }
    }
}
