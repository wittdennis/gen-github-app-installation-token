
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace Pseud0R4ndom.GenGithubAppInstallationToken;

internal class TokenGenerator : BackgroundService
{
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly ILogger _logger;
    private readonly GithubApiClient _githubApi;

    public TokenGenerator(IHostApplicationLifetime hostApplicationLifetime, GithubApiClient githubApi, ILogger<TokenGenerator> logger)
    {
        _hostApplicationLifetime = hostApplicationLifetime;
        _logger = logger;
        _githubApi = githubApi;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        string ghAppClientId = Environment.GetEnvironmentVariable("GITHUB_APP_CLIENT_ID") ?? "";
        string ghAppInstallationId = Environment.GetEnvironmentVariable("GITHUB_APP_INSTALLATION_ID") ?? "";
        string ghAppPrivateKey = Environment.GetEnvironmentVariable("GITHUB_APP_PRIVATE_KEY") ?? "";

        bool error = false;
        if (string.IsNullOrWhiteSpace(ghAppClientId))
        {
            _logger.LogError("GITHUB_APP_CLIENT_ID env variable not set.");
            error = true;
        }
        if (string.IsNullOrWhiteSpace(ghAppInstallationId))
        {
            _logger.LogError("GITHUB_APP_INSTALLATION_ID env variable not set.");
            error = true;
        }
        if (string.IsNullOrWhiteSpace(ghAppPrivateKey))
        {
            _logger.LogError("GITHUB_APP_PRIVATE_KEY env variable not set.");
            error = true;
        }
        if (error)
        {
            ReturnCode.Set(1);
            _hostApplicationLifetime.StopApplication();
            return;
        }


        using RSA rsaProvider = RSA.Create();
        rsaProvider.ImportFromPem(ghAppPrivateKey);
        SigningCredentials jwtSigningCreds = new SigningCredentials(new RsaSecurityKey(rsaProvider), SecurityAlgorithms.RsaSha256)
        {
            CryptoProviderFactory = new CryptoProviderFactory { CacheSignatureProviders = false },
        };

        GithubAppToken ghAppToken = GithubAppToken.Create(ghAppClientId);
        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Claims = ghAppToken.ToClaims(),
            Expires = ghAppToken.ExpiresAt.DateTime,
            IssuedAt = ghAppToken.IssuedAt.DateTime,
            NotBefore = ghAppToken.IssuedAt.DateTime,
            Issuer = ghAppToken.Issuer,
            TokenType = "JWT",
            SigningCredentials = jwtSigningCreds,
        };
        JwtSecurityTokenHandler jwtTokenHandler = new();
        SecurityToken jwtToken = jwtTokenHandler.CreateToken(tokenDescriptor);

        _githubApi.SetAuthorization(jwtTokenHandler.WriteToken(jwtToken));
        GithubApiResult<GithubInstallationToken> installationToken = await _githubApi.GetGithubAppInstallationToken(ghAppInstallationId, cancellationToken: stoppingToken);
        if (installationToken.Success)
        {
            Console.WriteLine(installationToken.Result!.Token);
        }
        else
        {
            _logger.LogError("Error calling github api. Received: {@Result}", installationToken.Error);
            ReturnCode.Set(1);
            return;
        }

        ReturnCode.Set(0);
        _hostApplicationLifetime.StopApplication();
    }
}
