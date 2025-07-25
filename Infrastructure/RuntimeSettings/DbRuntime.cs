using Microsoft.EntityFrameworkCore;

namespace Infrastructure.RuntimeSettings;

public abstract record DbRuntime
{
    public abstract DbContext DbContext { get; }
}