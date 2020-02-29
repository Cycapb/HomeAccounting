using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DomainModels.EntityORM.Core.Migrations
{
    public partial class InitMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    AccountID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountName = table.Column<string>(maxLength: 50, nullable: false),
                    Cash = table.Column<decimal>(type: "money", nullable: false),
                    Use = table.Column<bool>(nullable: false),
                    UserId = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.AccountID);
                });

            migrationBuilder.CreateTable(
                name: "NotificationMailBox",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MailBoxName = table.Column<string>(maxLength: 50, nullable: false),
                    MailFrom = table.Column<string>(maxLength: 50, nullable: false),
                    UserName = table.Column<string>(maxLength: 50, nullable: false),
                    Password = table.Column<string>(maxLength: 1024, nullable: false),
                    Server = table.Column<string>(maxLength: 50, nullable: false),
                    Port = table.Column<int>(nullable: false),
                    UseSsl = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationMailBox", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    OrderID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderDate = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(maxLength: 50, nullable: false),
                    Active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.OrderID);
                });

            migrationBuilder.CreateTable(
                name: "TypeOfFlow",
                columns: table => new
                {
                    TypeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeOfFlow", x => x.TypeID);
                });

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    CategoryID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    TypeOfFlowID = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(maxLength: 50, nullable: false),
                    ViewInPlan = table.Column<bool>(nullable: false),
                    Active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.CategoryID);
                    table.ForeignKey(
                        name: "FK_Category_TypeOfFlow_TypeOfFlowID",
                        column: x => x.TypeOfFlowID,
                        principalTable: "TypeOfFlow",
                        principalColumn: "TypeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Debt",
                columns: table => new
                {
                    DebtID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeOfFlowId = table.Column<int>(nullable: false),
                    AccountId = table.Column<int>(nullable: false),
                    Summ = table.Column<decimal>(type: "money", nullable: false),
                    Person = table.Column<string>(maxLength: 200, nullable: false),
                    DateBegin = table.Column<DateTime>(nullable: false),
                    DateEnd = table.Column<DateTime>(nullable: true),
                    UserId = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Debt", x => x.DebtID);
                    table.ForeignKey(
                        name: "FK_Debt_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "AccountID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Debt_TypeOfFlow_TypeOfFlowId",
                        column: x => x.TypeOfFlowId,
                        principalTable: "TypeOfFlow",
                        principalColumn: "TypeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PayingItem",
                columns: table => new
                {
                    ItemID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryID = table.Column<int>(nullable: false),
                    Summ = table.Column<decimal>(type: "money", nullable: false),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    AccountID = table.Column<int>(nullable: false),
                    Comment = table.Column<string>(maxLength: 1024, nullable: true),
                    UserId = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayingItem", x => x.ItemID);
                    table.ForeignKey(
                        name: "FK_PayingItem_Account_AccountID",
                        column: x => x.AccountID,
                        principalTable: "Account",
                        principalColumn: "AccountID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PayingItem_Category_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Category",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlanItem",
                columns: table => new
                {
                    PlanItemID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(nullable: false),
                    Month = table.Column<DateTime>(nullable: false),
                    SummPlan = table.Column<decimal>(type: "money", nullable: false),
                    SummFact = table.Column<decimal>(type: "money", nullable: false),
                    UserId = table.Column<string>(maxLength: 50, nullable: false),
                    Closed = table.Column<bool>(nullable: false),
                    IncomePlan = table.Column<decimal>(type: "money", nullable: false),
                    OutgoPlan = table.Column<decimal>(type: "money", nullable: false),
                    IncomeOutgoFact = table.Column<decimal>(type: "money", nullable: false),
                    BalanceFact = table.Column<decimal>(type: "money", nullable: false),
                    BalancePlan = table.Column<decimal>(type: "money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanItem", x => x.PlanItemID);
                    table.ForeignKey(
                        name: "FK_PlanItem_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    ProductID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(maxLength: 50, nullable: false),
                    CategoryID = table.Column<int>(nullable: false),
                    UserID = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.ProductID);
                    table.ForeignKey(
                        name: "FK_Product_Category_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Category",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetail",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    ProductPrice = table.Column<decimal>(type: "money", nullable: true),
                    Quantity = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetail", x => x.ID);
                    table.ForeignKey(
                        name: "FK_OrderDetail_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "OrderID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetail_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PayingItemProduct",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PayingItemId = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    ProductID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayingItemProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PayingItemProduct_PayingItem_PayingItemId",
                        column: x => x.PayingItemId,
                        principalTable: "PayingItem",
                        principalColumn: "ItemID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PayingItemProduct_Product_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Product",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Category_TypeOfFlowID",
                table: "Category",
                column: "TypeOfFlowID");

            migrationBuilder.CreateIndex(
                name: "IX_Debt_AccountId",
                table: "Debt",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Debt_TypeOfFlowId",
                table: "Debt",
                column: "TypeOfFlowId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_OrderId",
                table: "OrderDetail",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_ProductId",
                table: "OrderDetail",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PayingItem_AccountID",
                table: "PayingItem",
                column: "AccountID");

            migrationBuilder.CreateIndex(
                name: "IX_PayingItem_CategoryID",
                table: "PayingItem",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_PayingItemProduct_PayingItemId",
                table: "PayingItemProduct",
                column: "PayingItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PayingItemProduct_ProductID",
                table: "PayingItemProduct",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_PlanItem_CategoryId",
                table: "PlanItem",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_CategoryID",
                table: "Product",
                column: "CategoryID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Debt");

            migrationBuilder.DropTable(
                name: "NotificationMailBox");

            migrationBuilder.DropTable(
                name: "OrderDetail");

            migrationBuilder.DropTable(
                name: "PayingItemProduct");

            migrationBuilder.DropTable(
                name: "PlanItem");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "PayingItem");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "TypeOfFlow");
        }
    }
}
