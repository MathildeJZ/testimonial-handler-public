using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Testimonial.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTeacherEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropForeignKey(
            //     name: "FK_SubjectEntries_Teacher_TeacherId",
            //     table: "SubjectEntries");

            // migrationBuilder.DropForeignKey(
            //     name: "FK_TestimonialEntries_Teacher_TeacherId",
            //     table: "TestimonialEntries");

            // migrationBuilder.DropTable(
            //     name: "Teacher");

            // migrationBuilder.CreateTable(
            //     name: "Teachers",
            //     columns: table => new
            //     {
            //         TeacherId = table.Column<int>(type: "int", nullable: false)
            //             .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
            //         UserId = table.Column<string>(type: "longtext", nullable: false)
            //             .Annotation("MySql:CharSet", "utf8mb4"),
            //         FirstName = table.Column<string>(type: "longtext", nullable: false)
            //             .Annotation("MySql:CharSet", "utf8mb4"),
            //         LastName = table.Column<string>(type: "longtext", nullable: false)
            //             .Annotation("MySql:CharSet", "utf8mb4"),
            //         Email = table.Column<string>(type: "longtext", nullable: false)
            //             .Annotation("MySql:CharSet", "utf8mb4")
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Teacher", x => x.TeacherId);
            //     })
            // .Annotation("MySql:CharSet", "utf8mb4");

            // migrationBuilder.AddForeignKey(
            //     name: "FK_SubjectEntries_Teacher_TeacherId",
            //     table: "SubjectEntries",
            //     column: "TeacherId",
            //     principalTable: "Teacher",
            //     principalColumn: "TeacherId",
            //     onDelete: ReferentialAction.Cascade);

            // migrationBuilder.AddForeignKey(
            //     name: "FK_TestimonialEntries_Teacher_TeacherId",
            //     table: "TestimonialEntries",
            //     column: "TeacherId",
            //     principalTable: "Teacher",
            //     principalColumn: "TeacherId",
            //     onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubjectEntries_Teachers_TeacherId",
                table: "SubjectEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_TestimonialEntries_Teachers_TeacherId",
                table: "TestimonialEntries");

            migrationBuilder.DropTable(
                name: "Teachers");

            migrationBuilder.CreateTable(
                name: "Teacher",
                columns: table => new
                {
                    TeacherId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserId = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teacher", x => x.TeacherId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectEntries_Teacher_TeacherId",
                table: "SubjectEntries",
                column: "TeacherId",
                principalTable: "Teacher",
                principalColumn: "TeacherId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TestimonialEntries_Teacher_TeacherId",
                table: "TestimonialEntries",
                column: "TeacherId",
                principalTable: "Teacher",
                principalColumn: "TeacherId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
