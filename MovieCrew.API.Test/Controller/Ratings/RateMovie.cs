using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MovieCrew.API.Test.Controller.Ratings
{
    public class RateMovie
    {
        [Test]
        public async Task RateMovie()
        {
            RateController controller = new RateController();
            var actual = (await controller.RateMovie(1, 2, 3.0M)).Result as ObjectResult;

            Assert.That(actual.StatusCode, Is.EqualTo(StatusCodes.Status201Created));

        }
    }
}
