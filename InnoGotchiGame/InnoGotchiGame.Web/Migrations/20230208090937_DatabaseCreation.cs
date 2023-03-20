using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InnoGotchiGame.Web.Migrations
{
    /// <inheritdoc />
    public partial class DatabaseCreation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pictures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Format = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pictures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PasswordHach = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PictureId = table.Column<int>(type: "int", nullable: true),
                    OwnPetFarmId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Pictures_PictureId",
                        column: x => x.PictureId,
                        principalTable: "Pictures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ColaborationRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    RequestSenderId = table.Column<int>(type: "int", nullable: false),
                    RequestReceiverId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ColaborationRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ColaborationRequests_Users_RequestReceiverId",
                        column: x => x.RequestReceiverId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ColaborationRequests_Users_RequestSenderId",
                        column: x => x.RequestSenderId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PetFarms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OwnerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PetFarms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PetFarms_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatisticName = table.Column<string>(name: "Statistic_Name", type: "nvarchar(450)", nullable: false),
                    StatisticBornDate = table.Column<DateTime>(name: "Statistic_BornDate", type: "datetime2", nullable: false),
                    StatisticIsAlive = table.Column<bool>(name: "Statistic_IsAlive", type: "bit", nullable: false),
                    StatisticDeadDate = table.Column<DateTime>(name: "Statistic_DeadDate", type: "datetime2", nullable: true),
                    StatisticFeedingCount = table.Column<int>(name: "Statistic_FeedingCount", type: "int", nullable: false),
                    StatisticDrinkingCount = table.Column<int>(name: "Statistic_DrinkingCount", type: "int", nullable: false),
                    StatisticFirstHappinessDay = table.Column<DateTime>(name: "Statistic_FirstHappinessDay", type: "datetime2", nullable: false),
                    StatisticDateLastFeed = table.Column<DateTime>(name: "Statistic_DateLastFeed", type: "datetime2", nullable: false),
                    StatisticDateLastDrink = table.Column<DateTime>(name: "Statistic_DateLastDrink", type: "datetime2", nullable: false),
                    ViewPictureId = table.Column<int>(name: "View_PictureId", type: "int", nullable: true),
                    FarmId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pets_PetFarms_FarmId",
                        column: x => x.FarmId,
                        principalTable: "PetFarms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pets_Pictures_View_PictureId",
                        column: x => x.ViewPictureId,
                        principalTable: "Pictures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ColaborationRequests_RequestReceiverId",
                table: "ColaborationRequests",
                column: "RequestReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_ColaborationRequests_RequestSenderId",
                table: "ColaborationRequests",
                column: "RequestSenderId");

            migrationBuilder.CreateIndex(
                name: "IX_PetFarms_Name",
                table: "PetFarms",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PetFarms_OwnerId",
                table: "PetFarms",
                column: "OwnerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pets_FarmId",
                table: "Pets",
                column: "FarmId");

            migrationBuilder.CreateIndex(
                name: "IX_Pets_Statistic_Name",
                table: "Pets",
                column: "Statistic_Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pets_View_PictureId",
                table: "Pets",
                column: "View_PictureId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_PictureId",
                table: "Users",
                column: "PictureId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ColaborationRequests");

            migrationBuilder.DropTable(
                name: "Pets");

            migrationBuilder.DropTable(
                name: "PetFarms");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Pictures");
        }
    }
}
