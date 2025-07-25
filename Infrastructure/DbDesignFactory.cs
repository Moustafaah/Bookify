using Infrastructure.RuntimeSettings;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure;
internal class DbDesignFactory : IDesignTimeDbContextFactory<BookifyContext>
{
    public BookifyContext CreateDbContext(string[] args)
    {
        var c = Config.Default.Value.ConnectionString.Match(s => s, () => throw new InvalidOperationException($"ConnectionString could not be retrieved"));

        DbContextOptionsBuilder<BookifyContext> b = new DbContextOptionsBuilder<BookifyContext>().UseSqlServer(c);
        return new BookifyContext(b.Options);
    }
}
