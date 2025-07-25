using Domain.Apartments;
using Domain.Apartments.ValueObjects;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Name = Domain.Apartments.ValueObjects.Name;

namespace Infrastructure.Data.Configurations;

internal class ApartmentConfiguration : IEntityTypeConfiguration<Apartment>
{
    public void Configure(EntityTypeBuilder<Apartment> builder)
    {

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).HasColumnName("id");

        builder.Property(a => a.Name)
            .HasConversion(n => n.Repr, s => Name.FromUnsafe(s)).HasMaxLength(100).IsRequired().HasColumnName("name");

        builder.OwnsOne(a => a.Price, b =>
        {
            b.Property(m => m.Value).HasColumnName("price").HasColumnType("decimal(10,2)");
        });

        builder.Property<double?>("_cleaningFee").HasColumnName("cleaning_fee").HasColumnType("decimal(10,2)");

        builder.Property<DateTime?>("_lastBookedAt").HasColumnName("lastBookedAt");

        builder.Property<string?>("_description").HasColumnName("description").HasMaxLength(500);


        builder.Property(a => a.Currency).HasConversion(c => c.Code, s => Currency.FromUnsafe(s)).HasColumnName("currency").HasMaxLength(10).IsRequired();


        builder.OwnsMany(a => a.Amenities, b =>
        {
            b.ToTable("ApartmentAmenities");

            b.WithOwner().HasForeignKey("ApartmentId");

            b.Property<Guid>("Id"); // Required for owned collections
            b.HasKey("Id");

            b.Property(a => a.Name).HasConversion(n => n.Slug, s => Domain.Amenities.ValueObjects.Name.FromUnsafe(s)).HasColumnName("name").HasMaxLength(100);


            b.Property(a => a.State).HasConversion(s => s.Slug, s => Domain.Amenities.ValueObjects.State.FromUnsafe(s)).IsRequired().HasMaxLength(50).HasColumnName("state");





            b.Property<double?>("_percentage").HasColumnName("percentage").HasColumnType("decimal(4,2)");


            b.Property<double?>("_costPauschal").HasColumnName("cost_pauschal").HasColumnType("decimal(6,2)");

            b.Property<string?>("_description").HasColumnName("description").HasMaxLength(500);

            b.ToTable(tb => tb.HasCheckConstraint("CK_Apartment_Amenity_Cost_Pauschal", "[cost_pauschal] <= 10000"));

        });


        builder.HasMany(a => a.Bookings).WithOne(b => b.Apartment).HasForeignKey(b => b.ApartmentId).HasConstraintName("apartment_id");

        builder.ToTable(t => t.HasCheckConstraint("CK_Apartment_Price", "[price] <= 10000"));
        //builder.ToTable(t => t.HasCheckConstraint("CK_Apartment_Amenity_Cost_Pauschal", "[cost_pauschal] <= 10000"));


        builder.Property<uint>("version").IsConcurrencyToken();
    }
}
