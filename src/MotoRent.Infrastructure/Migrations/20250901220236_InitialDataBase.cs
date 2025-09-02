using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotoRent.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialDataBase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "MotoRent");

            migrationBuilder.CreateTable(
                name: "ConsumedMessages",
                schema: "MotoRent",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EventType = table.Column<string>(type: "text", nullable: false),
                    Payload = table.Column<string>(type: "text", nullable: false),
                    ReceivedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsumedMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Couriers",
                schema: "MotoRent",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Identifier = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Cnpj = table.Column<string>(type: "text", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DriverLicenseNumber = table.Column<string>(type: "text", nullable: false),
                    DriverLicenseType = table.Column<string>(type: "text", nullable: false),
                    DriverLicenseImage = table.Column<string>(type: "text", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Couriers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Motorcycles",
                schema: "MotoRent",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    Model = table.Column<string>(type: "text", nullable: false),
                    Identifier = table.Column<string>(type: "text", nullable: false),
                    LicensePlate = table.Column<string>(type: "text", nullable: false),
                    HasFine = table.Column<bool>(type: "boolean", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Motorcycles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LeaseContracts",
                schema: "MotoRent",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MotorcycleId = table.Column<Guid>(type: "uuid", nullable: false),
                    CourierId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeliveryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Amount = table.Column<decimal>(type: "numeric", nullable: true),
                    Fine = table.Column<decimal>(type: "numeric", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    ExpectedEndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpectedAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    Plan = table.Column<int>(type: "integer", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaseContracts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeaseContracts_Couriers_CourierId",
                        column: x => x.CourierId,
                        principalSchema: "MotoRent",
                        principalTable: "Couriers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LeaseContracts_Motorcycles_MotorcycleId",
                        column: x => x.MotorcycleId,
                        principalSchema: "MotoRent",
                        principalTable: "Motorcycles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Couriers_Cnpj",
                schema: "MotoRent",
                table: "Couriers",
                column: "Cnpj",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LeaseContracts_CourierId",
                schema: "MotoRent",
                table: "LeaseContracts",
                column: "CourierId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaseContracts_MotorcycleId",
                schema: "MotoRent",
                table: "LeaseContracts",
                column: "MotorcycleId");

            migrationBuilder.CreateIndex(
                name: "IX_Motorcycles_LicensePlate",
                schema: "MotoRent",
                table: "Motorcycles",
                column: "LicensePlate",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConsumedMessages",
                schema: "MotoRent");

            migrationBuilder.DropTable(
                name: "LeaseContracts",
                schema: "MotoRent");

            migrationBuilder.DropTable(
                name: "Couriers",
                schema: "MotoRent");

            migrationBuilder.DropTable(
                name: "Motorcycles",
                schema: "MotoRent");
        }
    }
}
