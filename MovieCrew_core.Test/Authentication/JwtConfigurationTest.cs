using System.Text;
using MovieCrew.Core.Domain.Authentication.Model;

namespace MovieCrew.Core.Test.Authentication;

public class JwtConfigurationTest
{
    [TestCase("blabl²__albalablablbla'@dsf232'^")]
    [TestCase("blaqdfqdf@@2413654Rlsjfsddsdddddd")]
    public void PassphraseShouldBeCorrect(string passphrase)
    {
        var configuration = new JwtConfiguration(passphrase, 1);
        Assert.That(Encoding.Unicode.GetByteCount(configuration.Passphrase), Is.GreaterThanOrEqualTo(64));
    }

    [TestCase("1223")]
    [TestCase("")]
    public void ShouldThrowExceptionWhenPassPhraseIsTooShort(string passphrase)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new JwtConfiguration(passphrase, 1));
    }

    [Test]
    public void ShouldThrowExceptionWhenPassPhraseIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new JwtConfiguration(null, 1));
    }

    [Test]
    public void ShouldThrowExceptionWhenNumberDaysBeforeExpirationZeroOrBelow()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new JwtConfiguration("", -1));
    }
}
