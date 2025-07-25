using Domain.Users;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;
internal class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(user => user.Id);
        builder.Property(u => u.Id).HasColumnName("id");


        builder.OwnsOne(u => u.Firstname, b =>
        {
            b.Property(f => f.Repr).HasColumnName("firstname").HasMaxLength(100).IsRequired();
        });

        builder.OwnsOne(u => u.Lastname, b =>
        {
            b.Property(l => l.Repr).HasColumnName("lastname").HasMaxLength(100).IsRequired();
        });
        builder.OwnsOne(u => u.Email, b =>
        {
            b.Property(e => e.Repr).HasColumnName("email").HasMaxLength(100).IsRequired();
            b.HasIndex(e => e.Repr).IsUnique();
        });




        builder.HasMany("_bookings")
            .WithOne("User").HasForeignKey("UserId");





    }
}
