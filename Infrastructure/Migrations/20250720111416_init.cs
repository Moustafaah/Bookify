using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Apartments",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    cleaning_fee = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    lastBookedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Apartments", x => x.id);
                    table.CheckConstraint("CK_Apartment_Price", "[price] <= 10000");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    firstname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    lastname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ApartmentAmenities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    state = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ApartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    cost_pauschal = table.Column<decimal>(type: "decimal(6,2)", nullable: true),
                    description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    percentage = table.Column<decimal>(type: "decimal(4,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApartmentAmenities", x => x.Id);
                    table.CheckConstraint("CK_Apartment_Amenity_Cost_Pauschal", "[cost_pauschal] <= 10000");
                    table.ForeignKey(
                        name: "FK_ApartmentAmenities_Apartments_ApartmentId",
                        column: x => x.ApartmentId,
                        principalTable: "Apartments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    start_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    end_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    price_for_period = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    amenities_up_charge = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    booking_status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Period_Day = table.Column<int>(type: "int", nullable: false),
                    Period_Week = table.Column<int>(type: "int", nullable: false),
                    Period_Month = table.Column<int>(type: "int", nullable: false),
                    Period_Year = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CancelledOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ConfirmedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeclinedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpiredOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CheckedInOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CheckedOutOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RejectedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NoShowOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RefundedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RescheduleOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    cleaning_fee = table.Column<decimal>(type: "decimal(10,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "apartment_id",
                        column: x => x.ApartmentId,
                        principalTable: "Apartments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApartmentAmenities_ApartmentId",
                table: "ApartmentAmenities",
                column: "ApartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ApartmentId",
                table: "Bookings",
                column: "ApartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_UserId",
                table: "Bookings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_email",
                table: "Users",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApartmentAmenities");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Apartments");
        }
    }
}
