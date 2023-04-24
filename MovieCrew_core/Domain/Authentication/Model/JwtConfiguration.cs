using System.Text;

namespace MovieCrew.Core.Domain.Authentication.Model;

public class JwtConfiguration
{
    private readonly string _passphrase;
    private readonly string _issuer;
    private readonly string _audience;
    private const int MinimumPassphraseBytesCount = 64; //see : https://www.rfc-editor.org/rfc/rfc7518#section-3.4

    public JwtConfiguration(){}

    public JwtConfiguration(string passphrase, string issuer, string audience)
    {
        _passphrase = passphrase;
        _issuer = issuer;
        _audience = audience;

    }

    public string Passphrase
    {
        get => _passphrase;
        init
        {
            var passphraseBytesCount = Encoding.Unicode.GetByteCount(value);
            if (passphraseBytesCount < MinimumPassphraseBytesCount)
            {
                throw new ArgumentOutOfRangeException(
                    $"{nameof(Passphrase)} should be at least {MinimumPassphraseBytesCount} bytes long. Actual is {passphraseBytesCount}");
            }
            _passphrase = value;
        }
    }

    public string Issuer
    {
        get => _issuer;
        init
        {
            CheckIfValidHost(value);
            _issuer = value;
        }
    }


    public string Audience
    {
        get => _audience;
        init
        {
            CheckIfValidHost(value);

            _audience = value;
        }
    }
    private static void CheckIfValidHost(string value)
    {
        var isValidHost = Uri.TryCreate(value, UriKind.Absolute, out Uri? uriResult)
                          && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

        if (!isValidHost) throw new ArgumentException($"{value} is not a valid host");
    }
}