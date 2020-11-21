using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ATARK.Migrations
{
    public partial class InitialCreate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KindsOfFishs",
                columns: table => new
                {
                    KindOfFishId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Kind = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KindsOfFishs", x => x.KindOfFishId);
                });

            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    OrganizationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Mail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FoundationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.OrganizationId);
                });

            migrationBuilder.CreateTable(
                name: "StatesOfSystems",
                columns: table => new
                {
                    StateOfTheSystemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Temperature = table.Column<float>(type: "real", nullable: false),
                    OxygenLevel = table.Column<float>(type: "real", nullable: false),
                    DateOfLastCheck = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatesOfSystems", x => x.StateOfTheSystemId);
                });

            migrationBuilder.CreateTable(
                name: "ClosedWaterSupplyInstallations",
                columns: table => new
                {
                    ClosedWaterSupplyInstallationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    StateOfTheSystemId = table.Column<int>(type: "int", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClosedWaterSupplyInstallations", x => x.ClosedWaterSupplyInstallationId);
                    table.ForeignKey(
                        name: "FK_ClosedWaterSupplyInstallations_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClosedWaterSupplyInstallations_StatesOfSystems_StateOfTheSystemId",
                        column: x => x.StateOfTheSystemId,
                        principalTable: "StatesOfSystems",
                        principalColumn: "StateOfTheSystemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pools",
                columns: table => new
                {
                    PoolId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClosedWaterSupplyInstallationId = table.Column<int>(type: "int", nullable: false),
                    WhoIsInThePool = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Volume = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pools", x => x.PoolId);
                    table.ForeignKey(
                        name: "FK_Pools_ClosedWaterSupplyInstallations_ClosedWaterSupplyInstallationId",
                        column: x => x.ClosedWaterSupplyInstallationId,
                        principalTable: "ClosedWaterSupplyInstallations",
                        principalColumn: "ClosedWaterSupplyInstallationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Fishs",
                columns: table => new
                {
                    FishId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KindOfFishId = table.Column<int>(type: "int", nullable: false),
                    Sex = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PoolNowId = table.Column<int>(type: "int", nullable: false),
                    RelocationPoolId = table.Column<int>(type: "int", nullable: false),
                    Weight = table.Column<float>(type: "real", nullable: false),
                    Adulthood = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fishs", x => x.FishId);
                    table.ForeignKey(
                        name: "FK_Fishs_KindsOfFishs_KindOfFishId",
                        column: x => x.KindOfFishId,
                        principalTable: "KindsOfFishs",
                        principalColumn: "KindOfFishId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Fishs_Pools_PoolNowId",
                        column: x => x.PoolNowId,
                        principalTable: "Pools",
                        principalColumn: "PoolId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Fishs_Pools_RelocationPoolId",
                        column: x => x.RelocationPoolId,
                        principalTable: "Pools",
                        principalColumn: "PoolId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Herds",
                columns: table => new
                {
                    HerdId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KindOfFishId = table.Column<int>(type: "int", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PoolIdNow = table.Column<int>(type: "int", nullable: false),
                    PoolId = table.Column<int>(type: "int", nullable: true),
                    AverageWeightOfAnIndividual = table.Column<float>(type: "real", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Herds", x => x.HerdId);
                    table.ForeignKey(
                        name: "FK_Herds_KindsOfFishs_KindOfFishId",
                        column: x => x.KindOfFishId,
                        principalTable: "KindsOfFishs",
                        principalColumn: "KindOfFishId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Herds_Pools_PoolId",
                        column: x => x.PoolId,
                        principalTable: "Pools",
                        principalColumn: "PoolId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Milkings",
                columns: table => new
                {
                    MilkingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FishId = table.Column<int>(type: "int", nullable: false),
                    MilkingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CaviarWeight = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Milkings", x => x.MilkingId);
                    table.ForeignKey(
                        name: "FK_Milkings_Fishs_FishId",
                        column: x => x.FishId,
                        principalTable: "Fishs",
                        principalColumn: "FishId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pregnancys",
                columns: table => new
                {
                    PregnancyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FishId = table.Column<int>(type: "int", nullable: false),
                    StartDateOfPregnancy = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pregnancys", x => x.PregnancyId);
                    table.ForeignKey(
                        name: "FK_Pregnancys_Fishs_FishId",
                        column: x => x.FishId,
                        principalTable: "Fishs",
                        principalColumn: "FishId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClosedWaterSupplyInstallations_OrganizationId",
                table: "ClosedWaterSupplyInstallations",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ClosedWaterSupplyInstallations_StateOfTheSystemId",
                table: "ClosedWaterSupplyInstallations",
                column: "StateOfTheSystemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Fishs_KindOfFishId",
                table: "Fishs",
                column: "KindOfFishId");

            migrationBuilder.CreateIndex(
                name: "IX_Fishs_PoolNowId",
                table: "Fishs",
                column: "PoolNowId");

            migrationBuilder.CreateIndex(
                name: "IX_Fishs_RelocationPoolId",
                table: "Fishs",
                column: "RelocationPoolId");

            migrationBuilder.CreateIndex(
                name: "IX_Herds_KindOfFishId",
                table: "Herds",
                column: "KindOfFishId");

            migrationBuilder.CreateIndex(
                name: "IX_Herds_PoolId",
                table: "Herds",
                column: "PoolId");

            migrationBuilder.CreateIndex(
                name: "IX_Milkings_FishId",
                table: "Milkings",
                column: "FishId");

            migrationBuilder.CreateIndex(
                name: "IX_Pools_ClosedWaterSupplyInstallationId",
                table: "Pools",
                column: "ClosedWaterSupplyInstallationId");

            migrationBuilder.CreateIndex(
                name: "IX_Pregnancys_FishId",
                table: "Pregnancys",
                column: "FishId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Herds");

            migrationBuilder.DropTable(
                name: "Milkings");

            migrationBuilder.DropTable(
                name: "Pregnancys");

            migrationBuilder.DropTable(
                name: "Fishs");

            migrationBuilder.DropTable(
                name: "KindsOfFishs");

            migrationBuilder.DropTable(
                name: "Pools");

            migrationBuilder.DropTable(
                name: "ClosedWaterSupplyInstallations");

            migrationBuilder.DropTable(
                name: "Organizations");

            migrationBuilder.DropTable(
                name: "StatesOfSystems");
        }
    }
}
