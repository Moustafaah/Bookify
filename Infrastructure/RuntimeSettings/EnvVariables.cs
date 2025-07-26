namespace Infrastructure.RuntimeSettings;

public record EnvVariables(string ConnectionString, string SendGridApiKey)
{
}
public record TestEnvVariables(string TestConnectionString, string SendGridApiKey)
{
}