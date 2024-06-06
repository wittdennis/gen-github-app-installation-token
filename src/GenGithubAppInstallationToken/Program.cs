using Pseud0R4ndom.GenGithubAppInstallationToken;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();
builder.Host.UseSerilog(dispose: true);
builder.Services.AddHttpClient();
builder.Services.AddHttpClient<GithubApiClient>();
builder.Services.AddHostedService<TokenGenerator>();

WebApplication app = builder.Build();
try
{
    app.Run();
}
catch { }

Environment.Exit(ReturnCode.Code);
