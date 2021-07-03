using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiAsMarket.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdminInfo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(maxLength: 200, nullable: true),
                    Mobile = table.Column<string>(maxLength: 11, nullable: true),
                    ImageUrl = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(maxLength: 50, nullable: true),
                    Password = table.Column<string>(maxLength: 150, nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    Token = table.Column<string>(maxLength: 300, nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    NationalCode = table.Column<string>(maxLength: 50, nullable: true),
                    Phone = table.Column<string>(maxLength: 50, nullable: true),
                    Commission = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    Family = table.Column<string>(maxLength: 50, nullable: true),
                    BirthDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Mobile = table.Column<string>(maxLength: 11, nullable: true),
                    Email = table.Column<string>(maxLength: 50, nullable: true),
                    PostalCode = table.Column<string>(maxLength: 10, nullable: true),
                    Address = table.Column<string>(maxLength: 300, nullable: true),
                    Image = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(maxLength: 50, nullable: true),
                    Password = table.Column<string>(maxLength: 150, nullable: true),
                    Token = table.Column<string>(maxLength: 300, nullable: true),
                    IsActive = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "File",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Date = table.Column<DateTime>(type: "datetime", nullable: true),
                    Link = table.Column<string>(maxLength: 1000, nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    Image = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_File", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Image",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Image", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Oprator",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    PersonalCode = table.Column<int>(nullable: true),
                    Mobile = table.Column<string>(maxLength: 11, nullable: true),
                    NationalCode = table.Column<string>(maxLength: 50, nullable: true),
                    UserName = table.Column<string>(maxLength: 50, nullable: true),
                    Password = table.Column<string>(maxLength: 100, nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    Family = table.Column<string>(maxLength: 500, nullable: true),
                    Image = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    Token = table.Column<string>(maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Oprator", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(nullable: false),
                    Amount = table.Column<long>(nullable: false),
                    Authority = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    Image1 = table.Column<string>(maxLength: 512, nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SaleFormSms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(nullable: false),
                    CustomerMobile = table.Column<string>(nullable: true),
                    OperatorId = table.Column<int>(nullable: false),
                    SellerId = table.Column<int>(nullable: false),
                    State = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleFormSms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Seller",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    PersonalCode = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    Mobile = table.Column<string>(maxLength: 11, nullable: true),
                    NationalCode = table.Column<string>(maxLength: 50, nullable: true),
                    Email = table.Column<string>(maxLength: 50, nullable: true),
                    Address = table.Column<string>(maxLength: 300, nullable: true),
                    Phone = table.Column<string>(maxLength: 50, nullable: true),
                    Commission = table.Column<long>(nullable: false),
                    Image = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(maxLength: 50, nullable: true),
                    Password = table.Column<string>(maxLength: 150, nullable: true),
                    Token = table.Column<string>(maxLength: 300, nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    Money = table.Column<long>(maxLength: 500, nullable: false),
                    Family = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seller", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ticket",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(maxLength: 500, nullable: true),
                    UserId = table.Column<int>(nullable: true),
                    SendDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Subject = table.Column<string>(maxLength: 50, nullable: true),
                    UserType = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    Mobile = table.Column<string>(maxLength: 11, nullable: true),
                    Status = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ticket", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TiketReciver",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReciverId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiketReciver", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    CategoryId = table.Column<int>(nullable: true),
                    Code = table.Column<string>(maxLength: 100, nullable: true),
                    Attribute = table.Column<string>(nullable: true),
                    Amount = table.Column<int>(nullable: true),
                    Takhfif = table.Column<int>(nullable: true),
                    Price = table.Column<long>(nullable: false),
                    PriceWithOff = table.Column<long>(nullable: false),
                    Image1 = table.Column<string>(nullable: true),
                    Image2 = table.Column<string>(nullable: true),
                    Image3 = table.Column<string>(nullable: true),
                    Commission = table.Column<long>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: true),
                    SellerId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Product_CategoryId_Category_Id",
                        column: x => x.CategoryId,
                        principalTable: "ProductCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Product_Seller_SellerId",
                        column: x => x.SellerId,
                        principalTable: "Seller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sale",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(nullable: false),
                    State = table.Column<int>(nullable: false),
                    SaleDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Commission = table.Column<long>(nullable: false),
                    TotalPrice = table.Column<long>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: true),
                    SellerId = table.Column<int>(nullable: false),
                    OperatorId = table.Column<int>(nullable: true),
                    TrackingCode = table.Column<string>(maxLength: 50, nullable: true),
                    IsFinally = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sale", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sale_CustomerId_Customer_Id",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sale_OperatorId_Operator_Id",
                        column: x => x.OperatorId,
                        principalTable: "Oprator",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sale_SellerId_Seller_Id",
                        column: x => x.SellerId,
                        principalTable: "Seller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FavoriteProduct",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(nullable: true),
                    UserId = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Favorite_ProductId_Product_Id",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Poster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    ProductId = table.Column<int>(nullable: true),
                    Image1 = table.Column<string>(nullable: true),
                    Image2 = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    Caption = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Poster", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Poster_ProductId_Product_Id",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SaleDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SaleId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    Count = table.Column<int>(nullable: false),
                    Price = table.Column<long>(nullable: false),
                    SellerId = table.Column<int>(nullable: false),
                    Commission = table.Column<long>(nullable: false),
                    ProductCode = table.Column<string>(nullable: true),
                    ProductName = table.Column<string>(nullable: true),
                    Takhfif = table.Column<int>(nullable: true),
                    DateTime = table.Column<DateTime>(nullable: true),
                    State = table.Column<int>(nullable: false),
                    IsAdmin = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaleDetail_ProductId_Product_Id",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SaleDetail_SaleId_Sale_Id",
                        column: x => x.SaleId,
                        principalTable: "Sale",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SaleDetail_SellerId_Seller_Id",
                        column: x => x.SellerId,
                        principalTable: "Seller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteProduct_ProductId",
                table: "FavoriteProduct",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Poster_ProductId",
                table: "Poster",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_CategoryId",
                table: "Product",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_SellerId",
                table: "Product",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_Sale_CustomerId",
                table: "Sale",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Sale_OperatorId",
                table: "Sale",
                column: "OperatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Sale_SellerId",
                table: "Sale",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleDetails_ProductId",
                table: "SaleDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleDetails_SaleId",
                table: "SaleDetails",
                column: "SaleId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleDetails_SellerId",
                table: "SaleDetails",
                column: "SellerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminInfo");

            migrationBuilder.DropTable(
                name: "FavoriteProduct");

            migrationBuilder.DropTable(
                name: "File");

            migrationBuilder.DropTable(
                name: "Image");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "Poster");

            migrationBuilder.DropTable(
                name: "SaleDetails");

            migrationBuilder.DropTable(
                name: "SaleFormSms");

            migrationBuilder.DropTable(
                name: "Ticket");

            migrationBuilder.DropTable(
                name: "TiketReciver");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Sale");

            migrationBuilder.DropTable(
                name: "ProductCategory");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "Oprator");

            migrationBuilder.DropTable(
                name: "Seller");
        }
    }
}
