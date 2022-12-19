namespace BillB0ard_API.Data.Models
{
    public class User
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Role { get; set; }

        //Navigation Properties
        public List<Rate>? Rates { get; set; }

    }
}
