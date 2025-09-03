using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Feed_Bridge.Migrations
{
    /// <inheritdoc />
    public partial class AddedPartenerAndStaticPageEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Parteners",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RedirectUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parteners", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Parteners_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StaticPages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VideoUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PartenerBackgroundImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HomePageBackgroundImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaticPages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StaticPages_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Parteners_UserId",
                table: "Parteners",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_StaticPages_UserId",
                table: "StaticPages",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Parteners");

            migrationBuilder.DropTable(
                name: "StaticPages");
        }
    }
}
