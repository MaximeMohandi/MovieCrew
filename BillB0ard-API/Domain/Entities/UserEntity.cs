namespace BillB0ard_API.Domain.Entities
{
    public record UserEntity(long Id, string Name, int Role = 0);
}