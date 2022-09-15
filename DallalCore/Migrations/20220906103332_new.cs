using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DallalCore.Migrations
{
    public partial class @new : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    addressID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    governorate = table.Column<string>(maxLength: 15, nullable: false),
                    district = table.Column<string>(maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.addressID);
                });

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    categoryID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    category = table.Column<string>(maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.categoryID);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    userID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsAdmin = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    Fname = table.Column<string>(maxLength: 15, nullable: false),
                    Lname = table.Column<string>(maxLength: 15, nullable: false),
                    Email = table.Column<string>(maxLength: 50, nullable: false),
                    UserPassword = table.Column<string>(nullable: false),
                    PhoneNumber = table.Column<string>(maxLength: 20, nullable: false),
                    WhatsApp = table.Column<string>(maxLength: 50, nullable: false),
                    addressId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.userID);
                    table.ForeignKey(
                        name: "FK_User_Address_addressId",
                        column: x => x.addressId,
                        principalTable: "Address",
                        principalColumn: "addressID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    productId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(maxLength: 100, nullable: false),
                    description = table.Column<string>(maxLength: 250, nullable: false),
                    pics_path = table.Column<string>(maxLength: 100, nullable: true),
                    price = table.Column<float>(nullable: true),
                    currency = table.Column<string>(maxLength: 50, nullable: true),
                    period_of_time = table.Column<string>(maxLength: 50, nullable: true),
                    dateTime = table.Column<DateTime>(nullable: false),
                    userId = table.Column<int>(nullable: true),
                    addressId = table.Column<int>(nullable: false),
                    categoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.productId);
                    table.ForeignKey(
                        name: "FK_Product_Address_addressId",
                        column: x => x.addressId,
                        principalTable: "Address",
                        principalColumn: "addressID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Product_Category_categoryId",
                        column: x => x.categoryId,
                        principalTable: "Category",
                        principalColumn: "categoryID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Product_User_userId",
                        column: x => x.userId,
                        principalTable: "User",
                        principalColumn: "userID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Rate",
                columns: table => new
                {
                    rateId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumberOfStars = table.Column<float>(nullable: false),
                    userId = table.Column<int>(nullable: true),
                    productId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rate", x => x.rateId);
                    table.ForeignKey(
                        name: "FK_Rate_Product_productId",
                        column: x => x.productId,
                        principalTable: "Product",
                        principalColumn: "productId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rate_User_userId",
                        column: x => x.userId,
                        principalTable: "User",
                        principalColumn: "userID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Report",
                columns: table => new
                {
                    reportID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<int>(nullable: true),
                    productId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Report", x => x.reportID);
                    table.ForeignKey(
                        name: "FK_Report_Product_productId",
                        column: x => x.productId,
                        principalTable: "Product",
                        principalColumn: "productId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Report_User_userId",
                        column: x => x.userId,
                        principalTable: "User",
                        principalColumn: "userID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Product_addressId",
                table: "Product",
                column: "addressId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_categoryId",
                table: "Product",
                column: "categoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_userId",
                table: "Product",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Rate_productId",
                table: "Rate",
                column: "productId");

            migrationBuilder.CreateIndex(
                name: "IX_Rate_userId",
                table: "Rate",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Report_productId",
                table: "Report",
                column: "productId");

            migrationBuilder.CreateIndex(
                name: "IX_Report_userId",
                table: "Report",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_User_addressId",
                table: "User",
                column: "addressId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rate");

            migrationBuilder.DropTable(
                name: "Report");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Address");
        }
    }
}
