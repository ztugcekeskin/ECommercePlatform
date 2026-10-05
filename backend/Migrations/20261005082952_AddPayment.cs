using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
{
    migrationBuilder.CreateTable(
        name: "Payments",
        columns: table => new
        {
            id = table.Column<int>(
                type: "integer",
                nullable: false
            )
            .Annotation(
                "Npgsql:ValueGenerationStrategy",
                NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
            ),

            orderid = table.Column<int>(
                type: "integer",
                nullable: false
            ),

            amount = table.Column<decimal>(
                type: "numeric",
                nullable: false
            ),

            paymentmethod = table.Column<string>(
                type: "text",
                nullable: false
            ),

            status = table.Column<string>(
                type: "text",
                nullable: false
            ),

            transactionid = table.Column<string>(
                type: "text",
                nullable: true
            ),

            createdat = table.Column<DateTime>(
                type: "timestamp with time zone",
                nullable: false
            )
        },
        constraints: table =>
        {
            table.PrimaryKey(
                "PK_Payments",
                x => x.id
            );

            table.ForeignKey(
                name: "FK_Payments_Orders_orderid",
                column: x => x.orderid,
                principalTable: "Orders",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade
            );
        }
    );

    migrationBuilder.CreateIndex(
        name: "IX_Payments_orderid",
        table: "Payments",
        column: "orderid",
        unique: true
    );
}

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
{
    migrationBuilder.DropTable(
        name: "Payments"
    );
}
    }
}
