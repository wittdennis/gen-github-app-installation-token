namespace Pseud0R4ndom.GenGithubAppInstallationToken;

internal class GithubApiResult<T>
{
    public T? Result { get; }
    public string? Error { get; }

    public bool Success => Result != null;

    public GithubApiResult(T result)
    {
        Result = result;
    }

    public GithubApiResult(string error)
    {
        Error = error;
    }
}
