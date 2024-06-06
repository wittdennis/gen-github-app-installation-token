namespace Pseud0R4ndom.GenGithubAppInstallationToken;

public static class ReturnCode
{
    private static int _code = 1;

    public static int Code => _code;

    public static void Set(int code)
    {
        _code = code;
    }
}
