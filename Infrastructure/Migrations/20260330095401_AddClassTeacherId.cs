using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Testimonial.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddClassTeacherId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            // migrationBuilder.AddForeignKey(
            //     name: "FK_Classes_Teacher_ClassTeacherId",
            //     table: "Classes",
            //     column: "ClassTeacherId",
            //     principalTable: "Teacher",
            //     principalColumn: "TeacherId",
            //     onDelete: ReferentialAction.Cascade);

            // migrationBuilder.DropColumn(
            //     name: "SubjectTeacherId",
            //     table: "Subjects");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Teacher_ClassTeacherId",
                table: "Classes");

            migrationBuilder.DropTable(
                name: "TeacherSubject");

            migrationBuilder.DropIndex(
                name: "IX_Classes_ClassTeacherId",
                table: "Classes");

            migrationBuilder.AddColumn<int>(
                name: "SubjectTeacherId",
                table: "Subjects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "SubjectId",
                keyValue: 1,
                column: "SubjectTeacherId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "SubjectId",
                keyValue: 2,
                column: "SubjectTeacherId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "SubjectId",
                keyValue: 3,
                column: "SubjectTeacherId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "SubjectId",
                keyValue: 4,
                column: "SubjectTeacherId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "SubjectId",
                keyValue: 5,
                column: "SubjectTeacherId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "SubjectId",
                keyValue: 6,
                column: "SubjectTeacherId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "SubjectId",
                keyValue: 7,
                column: "SubjectTeacherId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "SubjectId",
                keyValue: 8,
                column: "SubjectTeacherId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "SubjectId",
                keyValue: 9,
                column: "SubjectTeacherId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "SubjectId",
                keyValue: 10,
                column: "SubjectTeacherId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "SubjectId",
                keyValue: 11,
                column: "SubjectTeacherId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "SubjectId",
                keyValue: 12,
                column: "SubjectTeacherId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "SubjectId",
                keyValue: 13,
                column: "SubjectTeacherId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "SubjectId",
                keyValue: 14,
                column: "SubjectTeacherId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "SubjectId",
                keyValue: 15,
                column: "SubjectTeacherId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "SubjectId",
                keyValue: 16,
                column: "SubjectTeacherId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "SubjectId",
                keyValue: 17,
                column: "SubjectTeacherId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "SubjectId",
                keyValue: 18,
                column: "SubjectTeacherId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "SubjectId",
                keyValue: 19,
                column: "SubjectTeacherId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "SubjectId",
                keyValue: 20,
                column: "SubjectTeacherId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "SubjectId",
                keyValue: 21,
                column: "SubjectTeacherId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "SubjectId",
                keyValue: 22,
                column: "SubjectTeacherId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "SubjectId",
                keyValue: 23,
                column: "SubjectTeacherId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "SubjectId",
                keyValue: 24,
                column: "SubjectTeacherId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "SubjectId",
                keyValue: 25,
                column: "SubjectTeacherId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "SubjectId",
                keyValue: 26,
                column: "SubjectTeacherId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "SubjectId",
                keyValue: 27,
                column: "SubjectTeacherId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "SubjectId",
                keyValue: 28,
                column: "SubjectTeacherId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "SubjectId",
                keyValue: 29,
                column: "SubjectTeacherId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "SubjectId",
                keyValue: 30,
                column: "SubjectTeacherId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "SubjectId",
                keyValue: 31,
                column: "SubjectTeacherId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "SubjectId",
                keyValue: 32,
                column: "SubjectTeacherId",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Subjects",
                keyColumn: "SubjectId",
                keyValue: 33,
                column: "SubjectTeacherId",
                value: 0);
        }
    }
}
