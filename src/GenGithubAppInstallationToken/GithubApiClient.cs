using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;

namespace Pseud0R4ndom.GenGithubAppInstallationToken;

internal class GithubApiClient
{
    private readonly HttpClient _httpClient;
    private string _bearerToken = "";

    public GithubApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;

        string apiUrl = Environment.GetEnvironmentVariable("GITHUB_API_URL") ?? "https://api.github.com";
        _httpClient.BaseAddress = new Uri($"{apiUrl.TrimEnd('/')}/");
        _httpClient.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/vnd.github.v3+json");
        _httpClient.DefaultRequestHeaders.Add(HeaderNames.UserAgent, "Pseud0Random.GenGithubAppInstallationToken");
    }

    public void SetAuthorization(string ghAppToken)
    {
        _bearerToken = ghAppToken;
        _httpClient.DefaultRequestHeaders.Authorization = new("Bearer", ghAppToken);
    }

    public async Task<GithubApiResult<GithubInstallationToken>> GetGithubAppInstallationToken(string installationId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(_bearerToken))
        {
            throw new InvalidOperationException("Authorization must be set before making api calls");
        }

        HttpResponseMessage response = await _httpClient.PostAsync($"app/installations/{installationId.Trim()}/access_tokens", new StringContent(""), cancellationToken: cancellationToken);
        if (response.IsSuccessStatusCode)
        {
            GithubInstallationToken? token = await response.Content.ReadFromJsonAsync<GithubInstallationToken>();
            if (token == null)
            {
                return new GithubApiResult<GithubInstallationToken>("Error decoding Github Api response");
            }
            return new GithubApiResult<GithubInstallationToken>(token);
        }
        else
        {
            byte[] bytes = await response.Content.ReadAsByteArrayAsync();
            string rawBody = System.Text.Encoding.UTF8.GetString(bytes);
            return new GithubApiResult<GithubInstallationToken>(rawBody);
        }
    }
}
