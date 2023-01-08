using BillB0ard_API.Data.Models;
using BillB0ard_API.Domain.Entities;

namespace BillB0ard_API.Domain.Mappers
{
    public static class MovieMapperExtension
    {
        public static MovieEntity ToEntity(this Movie model)
        {
            return new(model.Id, model.Name, model.Poster, model.DateAdded, model.SeenDate);
        }
    }
}
