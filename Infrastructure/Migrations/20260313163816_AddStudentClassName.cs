using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Testimonial.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStudentClassName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
            name: "ClassName",
            table: "Students",
            type: "longtext",
            nullable: false,
            defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
            name: "ClassName",
            table: "Students");
        }
    }
}
