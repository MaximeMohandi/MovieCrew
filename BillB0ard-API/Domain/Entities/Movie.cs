namespace BillB0ard_API.Domain.Entities
{
    public record Movie(int Id, string Title, string? Poster, DateTime AddedDate, DateTime? SeenDate);
}
