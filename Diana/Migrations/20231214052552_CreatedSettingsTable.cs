using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Diana.Migrations
{
    public partial class CreatedSettingsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Logo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountIcon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Number1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Number2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Id", "AccountIcon", "Address", "Email", "Logo", "Number1", "Number2" },
                values: new object[] { 1, "<i class='fa fa-user-o'></i>", "Baku, Ayna Sultanova st. 221", "royal234@mail.ru", "assets/img/logo.png", "+994707094535", "+994773755354" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Settings");
        }
    }
}
