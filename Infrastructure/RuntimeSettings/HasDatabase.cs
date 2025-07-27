using Microsoft.EntityFrameworkCore;

namespace Infrastructure.RuntimeSettings;

public abstract record HasDatabase
{
    public abstract DbContext DbContext { get; }
}