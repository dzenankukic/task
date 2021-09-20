using Microsoft.EntityFrameworkCore.Migrations;

namespace task_V2.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Gradovi",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImeGrada = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gradovi", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Kandidati",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImeKandidata = table.Column<string>(nullable: true),
                    Kratica = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kandidati", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Rezultati",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GradoviID = table.Column<int>(nullable: false),
                    KandidatiID = table.Column<int>(nullable: false),
                    BrojGlasova = table.Column<int>(nullable: false),
                    Procenat = table.Column<decimal>(nullable: false),
                    isGreska = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rezultati", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Rezultati_Gradovi_GradoviID",
                        column: x => x.GradoviID,
                        principalTable: "Gradovi",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rezultati_Kandidati_KandidatiID",
                        column: x => x.KandidatiID,
                        principalTable: "Kandidati",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Kandidati",
                columns: new[] { "ID", "ImeKandidata", "Kratica" },
                values: new object[,]
                {
                    { 1, "Donald Trump", "DT" },
                    { 2, "Hillary Clinton", "HC" },
                    { 3, " Joe Biden", "JB" },
                    { 4, "John F. Kennedy", "JFK" },
                    { 5, "Jack Randall", "JR" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rezultati_GradoviID",
                table: "Rezultati",
                column: "GradoviID");

            migrationBuilder.CreateIndex(
                name: "IX_Rezultati_KandidatiID",
                table: "Rezultati",
                column: "KandidatiID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rezultati");

            migrationBuilder.DropTable(
                name: "Gradovi");

            migrationBuilder.DropTable(
                name: "Kandidati");
        }
    }
}
