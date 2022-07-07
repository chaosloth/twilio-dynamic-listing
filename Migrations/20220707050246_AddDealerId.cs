using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Twilio.Example.Migrations
{
    public partial class AddDealerId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DynamicNumber_Dealer_DealerId",
                table: "DynamicNumber");

            migrationBuilder.DropForeignKey(
                name: "FK_DynamicNumber_Listing_ListingId",
                table: "DynamicNumber");

            migrationBuilder.AlterColumn<int>(
                name: "ListingId",
                table: "DynamicNumber",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "DealerId",
                table: "DynamicNumber",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DynamicNumber_Dealer_DealerId",
                table: "DynamicNumber",
                column: "DealerId",
                principalTable: "Dealer",
                principalColumn: "DealerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DynamicNumber_Listing_ListingId",
                table: "DynamicNumber",
                column: "ListingId",
                principalTable: "Listing",
                principalColumn: "ListingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DynamicNumber_Dealer_DealerId",
                table: "DynamicNumber");

            migrationBuilder.DropForeignKey(
                name: "FK_DynamicNumber_Listing_ListingId",
                table: "DynamicNumber");

            migrationBuilder.AlterColumn<int>(
                name: "ListingId",
                table: "DynamicNumber",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DealerId",
                table: "DynamicNumber",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_DynamicNumber_Dealer_DealerId",
                table: "DynamicNumber",
                column: "DealerId",
                principalTable: "Dealer",
                principalColumn: "DealerId");

            migrationBuilder.AddForeignKey(
                name: "FK_DynamicNumber_Listing_ListingId",
                table: "DynamicNumber",
                column: "ListingId",
                principalTable: "Listing",
                principalColumn: "ListingId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
