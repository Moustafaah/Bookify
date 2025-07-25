using Domain.Abstraction;
using Domain.Apartments;
using Domain.Bookings;
using Domain.Users;

using Infrastructure.Outbox;

using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;

namespace Infrastructure;
public class BookifyContext : DbContext
{
    public BookifyContext(DbContextOptions<BookifyContext> options) : base(options) { }

    public DbSet<Apartment> Apartments { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<OutboxMessage> OutboxMessages { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(AssemblyRef.Assembly);

    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        await SaveDomainEvents();
        return await base.SaveChangesAsync(cancellationToken);
    }

    //private Task SaveDomainEvents()
    //{
    //    var outboxMessages = ChangeTracker.Entries<Entity>().Select(entry => entry.Entity).SelectMany(entity =>


    //        {
    //            var events = entity.GetDomainEvents();

    //            entity.ClearDomainEvents();
    //            return events;
    //        }
    //    ).Select(ev => new OutboxMessage()
    //    {
    //        Id = Guid.NewGuid(),
    //        OccuredOn = DateTime.Now,
    //        Type = ev.GetType().Name,
    //        Content = JsonConvert.SerializeObject(ev, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All })
    //    }).ToList();

    //    AddRange(outboxMessages);
    //    return Task.CompletedTask;
    //}

    private Task SaveDomainEvents()
    {
        var outboxMessages = ChangeTracker.Entries<Entity>()
            .Select(e => e.Entity)
            .SelectMany(entity =>
            {
                var domainEvents = entity.GetDomainEvents();
                foreach (var d in domainEvents)
                {
                    var events = entity.GetDomainEvents();
                    Console.WriteLine($"Entity  has {events.GetType().FullName} events.");
                    Console.WriteLine($"Entity  has evnt count{events.Count} events.");
                }
                entity.ClearDomainEvents();
                Console.WriteLine($"The Funckin DomainEvent is : {domainEvents.Count}");
                return domainEvents;
            })
            .Select(domainEvent =>
            {
                Console.WriteLine(domainEvent);
                return new OutboxMessage
                {

                    Id = Guid.NewGuid(),
                    OccuredOn = DateTime.UtcNow, // Better than DateTime.Now for consistency
                    Type = domainEvent.GetType().FullName!, // .FullName is better for distinguishing event types
                    Content = JsonConvert.SerializeObject(domainEvent, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    })
                };
            })
            .ToList();

        AddRange(outboxMessages);

        return Task.CompletedTask;
    }

}
