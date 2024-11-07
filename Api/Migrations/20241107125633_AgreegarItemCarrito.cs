using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class AgreegarItemCarrito : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Producto_Carrito_CarritoId",
                table: "Producto");

            migrationBuilder.DropIndex(
                name: "IX_Producto_CarritoId",
                table: "Producto");

            migrationBuilder.DropColumn(
                name: "CarritoId",
                table: "Producto");

            migrationBuilder.CreateTable(
                name: "ItemCarrito",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ProductoId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    Subtotal = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    CarritoId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemCarrito", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemCarrito_Carrito_CarritoId",
                        column: x => x.CarritoId,
                        principalTable: "Carrito",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemCarrito_Producto_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Producto",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ItemCarrito_CarritoId",
                table: "ItemCarrito",
                column: "CarritoId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemCarrito_ProductoId",
                table: "ItemCarrito",
                column: "ProductoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemCarrito");

            migrationBuilder.AddColumn<Guid>(
                name: "CarritoId",
                table: "Producto",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_Producto_CarritoId",
                table: "Producto",
                column: "CarritoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Producto_Carrito_CarritoId",
                table: "Producto",
                column: "CarritoId",
                principalTable: "Carrito",
                principalColumn: "Id");
        }
    }
}
