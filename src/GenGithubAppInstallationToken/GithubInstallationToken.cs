using System.Text.Json.Serialization;

namespace Pseud0R4ndom.GenGithubAppInstallationToken;

internal class GithubInstallationToken
{
    [JsonPropertyName("token")]
    public string Token { get; set; } = "";
}
