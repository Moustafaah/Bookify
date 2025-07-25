namespace Infrastructure.RuntimeSettings;
public record RuntimeEnv(HttpClient HttpClient, Config Config) : IDisposable
{
    public void Dispose()
    {
        HttpClient.Dispose();
    }

}