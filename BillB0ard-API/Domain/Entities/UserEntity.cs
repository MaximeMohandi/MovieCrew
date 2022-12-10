namespace BillB0ard_API.Domain.Entities
{
    public record UserEntity(long id, string name, int role = 0);
}