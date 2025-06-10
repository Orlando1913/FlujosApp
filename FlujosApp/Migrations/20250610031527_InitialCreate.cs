using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlujosApp.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Flujos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flujos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pasos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Orden = table.Column<int>(type: "int", nullable: false),
                    FlujoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pasos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pasos_Flujos_FlujoId",
                        column: x => x.FlujoId,
                        principalTable: "Flujos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Campos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Valor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    YaProcesado = table.Column<bool>(type: "bit", nullable: false),
                    PasoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Campos_Pasos_PasoId",
                        column: x => x.PasoId,
                        principalTable: "Pasos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PasoDependencias",
                columns: table => new
                {
                    PasoId = table.Column<int>(type: "int", nullable: false),
                    DependeDePasoId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasoDependencias", x => new { x.PasoId, x.DependeDePasoId });
                    table.ForeignKey(
                        name: "FK_PasoDependencias_Pasos_DependeDePasoId",
                        column: x => x.DependeDePasoId,
                        principalTable: "Pasos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PasoDependencias_Pasos_PasoId",
                        column: x => x.PasoId,
                        principalTable: "Pasos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Campos_PasoId",
                table: "Campos",
                column: "PasoId");

            migrationBuilder.CreateIndex(
                name: "IX_PasoDependencias_DependeDePasoId",
                table: "PasoDependencias",
                column: "DependeDePasoId");

            migrationBuilder.CreateIndex(
                name: "IX_Pasos_FlujoId",
                table: "Pasos",
                column: "FlujoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Campos");

            migrationBuilder.DropTable(
                name: "PasoDependencias");

            migrationBuilder.DropTable(
                name: "Pasos");

            migrationBuilder.DropTable(
                name: "Flujos");
        }
    }
}
