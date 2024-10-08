using System.Text;

namespace MovieCrew.Core.Domain.Authentication.Model;

public class JwtConfiguration
{
    private const int MinimumPassphraseBytesCount = 64; //see : https://www.rfc-editor.org/rfc/rfc7518#section-3.4
    private readonly string _audience;
    private readonly string _issuer;
    private readonly int _maxTokenValidationDays;
    private readonly string _passphrase;

    public JwtConfiguration()
    {
    }

    public JwtConfiguration(string passphrase, string issuer, string audience, int maxTokenValidationDays)
    {
        Passphrase = passphrase;
        Issuer = issuer;
        Audience = audience;
        MaxTokenValidationDays = maxTokenValidationDays;
    }

    public string Passphrase
    {
        get => _passphrase;
        init
        {
            var passphraseBytesCount = Encoding.Unicode.GetByteCount(value);
            if (passphraseBytesCount < MinimumPassphraseBytesCount)
                throw new ArgumentOutOfRangeException(
                    $"{nameof(Passphrase)} should be at least {MinimumPassphraseBytesCount} bytes long. Actual is {passphraseBytesCount}");
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

    public int MaxTokenValidationDays
    {
        get => _maxTokenValidationDays;
        init
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException($"{nameof(MaxTokenValidationDays)} should be greater than 0");
            _maxTokenValidationDays = value;
        }
    }

    private static void CheckIfValidHost(string value)
    {
        var isValidHost = Uri.TryCreate(value, UriKind.Absolute, out var uriResult)
                          && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

        if (!isValidHost) throw new ArgumentException($"{value} is not a valid host");
    }
}
