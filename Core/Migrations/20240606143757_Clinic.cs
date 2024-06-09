using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class Clinic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ClinicId",
                table: "DentistDetails",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_DentistDetails_ClinicId",
                table: "DentistDetails",
                column: "ClinicId");

            migrationBuilder.AddForeignKey(
                name: "FK_DentistDetails_Clinics_ClinicId",
                table: "DentistDetails",
                column: "ClinicId",
                principalTable: "Clinics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DentistDetails_Clinics_ClinicId",
                table: "DentistDetails");

            migrationBuilder.DropIndex(
                name: "IX_DentistDetails_ClinicId",
                table: "DentistDetails");

            migrationBuilder.DropColumn(
                name: "ClinicId",
                table: "DentistDetails");
        }
    }
}
