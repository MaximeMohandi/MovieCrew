﻿using System.Text;
using MovieCrew.Core.Domain.Authentication.Model;

namespace MovieCrew.Core.Test.Authentication;

public class JwtConfigurationTest
{
    [TestCase("blabl²__albalablablbla'@dsf232'^")]
    [TestCase("blaqdfqdf@@2413654Rlsjfsddsdddddd")]
    public void PassphraseShouldBeCorrect(string passphrase)
    {
        var configuration = new JwtConfiguration(passphrase, "https://test.com", "https://test.com", 1);
        Assert.That(Encoding.Unicode.GetByteCount(configuration.Passphrase), Is.GreaterThanOrEqualTo(64));
    }

    [TestCase("1223")]
    [TestCase("")]
    public void ShouldThrowExceptionWhenPassPhraseIsTooShort(string passphrase)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new JwtConfiguration
        {
            Passphrase = passphrase
        });
    }

    [Test]
    public void ShouldThrowExceptionWhenPassPhraseIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new JwtConfiguration
        {
            Passphrase = null
        });
    }

    [Test]
    public void IssuerAndAudienceShouldBeCorrectUrl()
    {
        var configuration = new JwtConfiguration("blaqdfqdf@@2413654Rlsjfsddsdddddd", "https://host.fr",
            "https://audience.com", 1);

        Assert.Multiple(() =>
        {
            Assert.That(Uri.TryCreate(configuration.Issuer, UriKind.Absolute, out var issuerUriResult)
                        && (issuerUriResult.Scheme == Uri.UriSchemeHttp ||
                            issuerUriResult.Scheme == Uri.UriSchemeHttps), Is.True);
            Assert.That(Uri.TryCreate(configuration.Audience, UriKind.Absolute, out var audienceUriResult)
                        && (audienceUriResult.Scheme == Uri.UriSchemeHttp ||
                            audienceUriResult.Scheme == Uri.UriSchemeHttps), Is.True);
        });
    }

    [TestCase(null)]
    [TestCase("sdlmkfj")]
    [TestCase("http:/dfdf.com")]
    public void ShouldThrowExceptionWhenIssuerIsNotValid(string issuer)
    {
        Assert.Throws<ArgumentException>(() => new JwtConfiguration
            {
                Issuer = issuer
            }, $"{issuer} is not a valid host");
    }

    [Test]
    public void ShouldThrowExceptionWhenNumberDaysBeforeExpirationZeroOrBelow()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new JwtConfiguration("", "", "", -1));
    }
}
