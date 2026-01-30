using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BibliotecaBlazor.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "estudiantes",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    cedula = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    apellido = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    semestre = table.Column<int>(type: "integer", nullable: false),
                    carrera = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    seccion = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    activo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_estudiantes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "libros",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    titulolibro = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    autor = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    cantidaddisponible = table.Column<int>(type: "integer", nullable: false),
                    caratulaurl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    activo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_libros", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "prestamos",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    estudianteid = table.Column<int>(type: "integer", nullable: false),
                    esinterno = table.Column<bool>(type: "boolean", nullable: false),
                    fechasalida = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    fechadevolucion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    observaciones = table.Column<string>(type: "text", nullable: true),
                    devuelto = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_prestamos", x => x.id);
                    table.ForeignKey(
                        name: "FK_prestamos_estudiantes_estudianteid",
                        column: x => x.estudianteid,
                        principalTable: "estudiantes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "devoluciones",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    prestamoid = table.Column<int>(type: "integer", nullable: false),
                    fechadevolucion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    observaciones = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_devoluciones", x => x.id);
                    table.ForeignKey(
                        name: "FK_devoluciones_prestamos_prestamoid",
                        column: x => x.prestamoid,
                        principalTable: "prestamos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "prestamosdetalles",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    prestamoid = table.Column<int>(type: "integer", nullable: false),
                    libroid = table.Column<int>(type: "integer", nullable: false),
                    cantidad = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_prestamosdetalles", x => x.id);
                    table.ForeignKey(
                        name: "FK_prestamosdetalles_libros_libroid",
                        column: x => x.libroid,
                        principalTable: "libros",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_prestamosdetalles_prestamos_prestamoid",
                        column: x => x.prestamoid,
                        principalTable: "prestamos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_devoluciones_prestamoid",
                table: "devoluciones",
                column: "prestamoid");

            migrationBuilder.CreateIndex(
                name: "ix_prestamos_estudianteid",
                table: "prestamos",
                column: "estudianteid");

            migrationBuilder.CreateIndex(
                name: "ix_prestamosdetalles_libroid",
                table: "prestamosdetalles",
                column: "libroid");

            migrationBuilder.CreateIndex(
                name: "ix_prestamosdetalles_prestamoid",
                table: "prestamosdetalles",
                column: "prestamoid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "devoluciones");

            migrationBuilder.DropTable(
                name: "prestamosdetalles");

            migrationBuilder.DropTable(
                name: "libros");

            migrationBuilder.DropTable(
                name: "prestamos");

            migrationBuilder.DropTable(
                name: "estudiantes");
        }
    }
}
