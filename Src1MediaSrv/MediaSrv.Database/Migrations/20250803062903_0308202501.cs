using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieCDN.Database.Migrations
{
    /// <inheritdoc />
    public partial class _0308202501 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "secretKey",
                table: "ApiKeys",
                newName: "SecretKey");

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(100)", nullable: false),
                    PartitionKey = table.Column<string>(type: "varchar(100)", nullable: false),
                    FileName = table.Column<string>(type: "varchar(255)", nullable: false),
                    Type = table.Column<string>(type: "varchar(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.RenameColumn(
                name: "SecretKey",
                table: "ApiKeys",
                newName: "secretKey");
        }
    }
}
