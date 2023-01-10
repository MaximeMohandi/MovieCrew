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

        public static Movie ToModel(this MovieEntity entity)
        {
            return new Movie
            {
                Id = entity.Id,
                Name = entity.Title,
                DateAdded = entity.AddedDate,
                Poster = entity.Poster,
                SeenDate = entity.SeenDate
            };
        }
    }
}
