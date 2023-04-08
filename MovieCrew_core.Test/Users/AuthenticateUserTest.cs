namespace MovieCrew_Core.Users
{
    public class AuthenticateUserTest
    {
        [Test]
        public void AuthenticateUser()
        {

            var actual = new
            {
                TokenId = Guid.NewGuid(),
                Token = "ZAA",
                RefreshToken = "ZAAA",
                TokenExpirationDate = new DateTimeOffset(DateTime.Now.AddHours(2)).DateTime,
                RefreshTokenExpirationDate = new DateTimeOffset(DateTime.Now.AddDays(1)).DateTime,
            };
            Assert.Multiple(() =>
            {
                Assert.That(actual.Token, Is.Not.Empty);
                Assert.That(actual.RefreshToken, Is.Not.Empty);
                Assert.That(actual.TokenExpirationDate, Is.EqualTo(new DateTimeOffset(DateTime.Now.AddHours(2)).DateTime));
                Assert.That(actual.RefreshTokenExpirationDate, Is.EqualTo(new DateTimeOffset(DateTime.Now.AddDays(1)).DateTime));
            });
        }
    }
}
