
using Infrastructure.RuntimeSettings;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.DbRuntime;
public record BookifyRT : RuntimeSettings.DbRuntime, IDisposable, IAsyncDisposable
{
    public override BookifyContext DbContext { get; }
    public BookifyRT()
    {
        DbContext = new BookifyContext(
            options: new DbContextOptionsBuilder<BookifyContext>()
                .UseSqlServer(
                    Config.Default.Value.ConnectionString
                        .Match(s => s, () => throw new InvalidOperationException($"Db ConnectionString could not be retrieved")))
                .EnableSensitiveDataLogging()
                .LogTo(Console.WriteLine, LogLevel.Information)
                .Options);
    }


    public void Dispose()
    {
        DbContext.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await DbContext.DisposeAsync();
    }


}



