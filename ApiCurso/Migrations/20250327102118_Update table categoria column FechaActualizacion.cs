using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiCurso.Migrations
{
    /// <inheritdoc />
    public partial class UpdatetablecategoriacolumnFechaActualizacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FechaActualizacion",
                table: "Categorias",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaActualizacion",
                table: "Categorias");
        }
    }
}
