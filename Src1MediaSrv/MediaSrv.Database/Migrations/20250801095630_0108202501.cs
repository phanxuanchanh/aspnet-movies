using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieCDN.Database.Migrations
{
    /// <inheritdoc />
    public partial class _0108202501 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApiKeys",
                columns: table => new
                {
                    ClientId = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    secretKey = table.Column<string>(type: "nvarchar(500)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiKeys", x => x.ClientId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApiKeys");
        }
    }
}
