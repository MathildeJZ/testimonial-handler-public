using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Testimonial.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTeacherRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SchoolName",
                table: "Students",
                newName: "School");

            migrationBuilder.AddColumn<bool>(
                name: "IsClassTeacher",
                table: "Teacher",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSubjectTeacher",
                table: "Teacher",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsClassTeacher",
                table: "Teacher");

            migrationBuilder.DropColumn(
                name: "IsSubjectTeacher",
                table: "Teacher");

            migrationBuilder.RenameColumn(
                name: "School",
                table: "Students",
                newName: "SchoolName");
        }
    }
}
