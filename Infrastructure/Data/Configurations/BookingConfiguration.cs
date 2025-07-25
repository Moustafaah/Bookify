using Domain.Bookings;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;
internal class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.HasKey(b => b.Id);

        //builder.HasOne(b => b.User).WithMany(u => u.Bookings).HasForeignKey(b => b.UserId);

        builder.HasOne(b => b.Apartment).WithMany(u => u.Bookings).HasForeignKey(b => b.ApartmentId);


        builder.OwnsOne(b => b.Duration, b =>
        {
            b.Property(d => d.FromDate).HasColumnName("start_date").IsRequired();
            b.Property(d => d.ToDate).HasColumnName("end_date").IsRequired();

        });

        builder.Property<double?>("_cleaningFee").HasColumnName("cleaning_fee").HasColumnType("decimal(10,2)");

        builder.OwnsOne(a => a.PriceForPeriod, b =>
        {
            b.Property(m => m.Value).HasColumnName("price_for_period").HasColumnType("decimal(10,2)");
        });

        builder.OwnsOne(a => a.AmenitiesUpCharge, b =>
        {
            b.Property(m => m.Value).HasColumnName("amenities_up_charge").HasColumnType("decimal(10,2)");
        });


        builder.OwnsOne(e => e.BookingStatus, b =>
        {
            b.OwnsOne(bs => bs.Status, s =>
            {
                s.Property(p => p.Name)
                    .HasColumnName("booking_status")
                    .HasMaxLength(50)
                    .IsRequired();
            });

        });
        builder.OwnsOne(b => b.Period, b =>
        {
            b.Property(p => p.Day);
            b.Property(p => p.Week);
            b.Property(p => p.Month);
            b.Property(p => p.Year);
        });

    }
}
