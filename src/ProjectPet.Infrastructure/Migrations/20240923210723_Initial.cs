using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectPet.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "volunteers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    full_name = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    email = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    description = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    yo_experience = table.Column<int>(type: "integer", nullable: false),
                    area_code = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    number = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    payment_methods = table.Column<string>(type: "jsonb", nullable: true),
                    social_networks = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_volunteers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "pets",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    species = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    description = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    breed = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    coat = table.Column<string>(type: "text", nullable: false),
                    phone_number = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    date_of_birth = table.Column<DateOnly>(type: "date", nullable: false),
                    created_on = table.Column<DateOnly>(type: "date", nullable: false),
                    pet_id = table.Column<Guid>(type: "uuid", nullable: true),
                    apartment = table.Column<int>(type: "integer", nullable: false),
                    block = table.Column<string>(type: "text", nullable: true),
                    building = table.Column<string>(type: "text", nullable: false),
                    entrance = table.Column<int>(type: "integer", nullable: true),
                    floor = table.Column<int>(type: "integer", nullable: true),
                    saved_name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    street = table.Column<string>(type: "text", nullable: false),
                    health_info = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    height = table.Column<float>(type: "real", nullable: false),
                    is_sterilized = table.Column<bool>(type: "boolean", nullable: false),
                    is_vaccinated = table.Column<bool>(type: "boolean", nullable: false),
                    weight = table.Column<float>(type: "real", nullable: false),
                    payment_methods = table.Column<string>(type: "jsonb", nullable: true),
                    photos = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_pets", x => x.id);
                    table.ForeignKey(
                        name: "fk_pets_volunteers_pet_id",
                        column: x => x.pet_id,
                        principalTable: "volunteers",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_pets_pet_id",
                table: "pets",
                column: "pet_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "pets");

            migrationBuilder.DropTable(
                name: "volunteers");
        }
    }
}
