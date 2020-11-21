using Microsoft.EntityFrameworkCore.Migrations;

namespace ATARK.Migrations
{
    public partial class InitialCreate3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClosedWaterSupplyInstallations_StatesOfSystems_StateOfTheSystemId",
                table: "ClosedWaterSupplyInstallations");

            migrationBuilder.AddForeignKey(
                name: "FK_ClosedWaterSupplyInstallations_StatesOfSystems_StateOfTheSystemId",
                table: "ClosedWaterSupplyInstallations",
                column: "StateOfTheSystemId",
                principalTable: "StatesOfSystems",
                principalColumn: "StateOfTheSystemId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClosedWaterSupplyInstallations_StatesOfSystems_StateOfTheSystemId",
                table: "ClosedWaterSupplyInstallations");

            migrationBuilder.AddForeignKey(
                name: "FK_ClosedWaterSupplyInstallations_StatesOfSystems_StateOfTheSystemId",
                table: "ClosedWaterSupplyInstallations",
                column: "StateOfTheSystemId",
                principalTable: "StatesOfSystems",
                principalColumn: "StateOfTheSystemId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
