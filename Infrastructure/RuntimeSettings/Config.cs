using System.Text.Json;

using Microsoft.Extensions.Configuration.UserSecrets;

namespace Infrastructure.RuntimeSettings;


public record Config(Option<string> ConnectionString, Option<string> SendGridApiKey)
{
    public static Lazy<Config> Default = new Lazy<Config>(() =>
    {
        var secrets = LoadSecrets();
        return new Config(secrets.ConnectionString, secrets.SendGridApiKey);
    });

    public static Lazy<Config> Test = new Lazy<Config>(() =>
    {
        var secrets = LoadTestSecrets();
        return new Config(secrets.ConnectionString, secrets.SendGridApiKey);
    });
    private static (Option<string> ConnectionString, Option<string> SendGridApiKey) LoadSecrets()
    {

        Console.WriteLine("Loading Secrets!!!!!!!!!!!!!!!!!!!!!!");
        var id = "0e05e917-0f5e-4d17-a9b2-44b47b9c507f";
        var path = PathHelper.GetSecretsPathFromSecretsId(id);
        var text = File.ReadAllText(path);
        var env = JsonSerializer.Deserialize<EnvVariables>(text);
        return Optional(env).Match(e => (Optional(e.ConnectionString), Optional(e.SendGridApiKey)), () => (Option<string>.None, Option<string>.None));
    }

    private static (Option<string> ConnectionString, Option<string> SendGridApiKey) LoadTestSecrets()
    {

        Console.WriteLine("Loading Test Secrets!!!!!!!!!!!!!!!!!!!!!!");
        var id = "0e05e917-0f5e-4d17-a9b2-44b47b9c507f";
        var path = PathHelper.GetSecretsPathFromSecretsId(id);
        var text = File.ReadAllText(path);
        var env = JsonSerializer.Deserialize<TestEnvVariables>(text);
        return Optional(env).Match(e => (Optional(e.TestConnectionString), Optional(e.SendGridApiKey)), () => (Option<string>.None, Option<string>.None));
    }
}



