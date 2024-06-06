namespace Pseud0R4ndom.GenGithubAppInstallationToken;

internal class GithubAppToken
{
    private GithubAppToken() { }

    public DateTimeOffset IssuedAt { get; internal set; }

    public DateTimeOffset ExpiresAt { get; internal set; }

    public string Issuer { get; internal set; } = "";

    public static GithubAppToken Create(string ghAppClientId)
        => new()
        {
            IssuedAt = DateTimeOffset.Now.AddSeconds(-60),
            ExpiresAt = DateTimeOffset.Now.AddMinutes(10),
            Issuer = ghAppClientId,
        };

    public virtual IDictionary<string, object> ToClaims()
    {
        Dictionary<string, object> claims = new()
        {
            { "iat", IssuedAt.ToUnixTimeSeconds() },
            { "exp", ExpiresAt.ToUnixTimeSeconds() },
            { "iss", Issuer },
        };

        return claims;
    }
}
