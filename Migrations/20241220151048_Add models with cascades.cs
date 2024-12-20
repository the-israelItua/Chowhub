using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChowHub.Migrations
{
    /// <inheritdoc />
    public partial class Addmodelswithcascades : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Customers_UserId",
                table: "Carts");

            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Restaurants_RestaurantId",
                table: "Carts");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Customers_UserId",
                table: "Carts",
                column: "UserId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Restaurants_RestaurantId",
                table: "Carts",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Customers_UserId",
                table: "Carts");

            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Restaurants_RestaurantId",
                table: "Carts");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Customers_UserId",
                table: "Carts",
                column: "UserId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Restaurants_RestaurantId",
                table: "Carts",
                column: "RestaurantId",
                principalTable: "Restaurants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
