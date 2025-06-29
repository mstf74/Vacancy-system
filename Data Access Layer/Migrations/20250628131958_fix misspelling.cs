using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data_Access_Layer.Migrations
{
    /// <inheritdoc />
    public partial class fixmisspelling : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_vacancyApplications_vacancies_VaccancyId",
                table: "vacancyApplications");

            migrationBuilder.RenameColumn(
                name: "VaccancyId",
                table: "vacancyApplications",
                newName: "VacancyId");

            migrationBuilder.AddForeignKey(
                name: "FK_vacancyApplications_vacancies_VacancyId",
                table: "vacancyApplications",
                column: "VacancyId",
                principalTable: "vacancies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_vacancyApplications_vacancies_VacancyId",
                table: "vacancyApplications");

            migrationBuilder.RenameColumn(
                name: "VacancyId",
                table: "vacancyApplications",
                newName: "VaccancyId");

            migrationBuilder.AddForeignKey(
                name: "FK_vacancyApplications_vacancies_VaccancyId",
                table: "vacancyApplications",
                column: "VaccancyId",
                principalTable: "vacancies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
