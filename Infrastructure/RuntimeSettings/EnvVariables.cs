namespace Infrastructure.RuntimeSettings;

public record EnvVariables(string ConnectionString, string SendGridApiKey)
{
}