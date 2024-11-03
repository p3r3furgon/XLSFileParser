using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace B1Task2.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "accountsource",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sourcetype = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    uploaddate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("accountsource_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "det",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("det_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "accountclass",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    classcode = table.Column<int>(type: "integer", nullable: false),
                    classname = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    SourceId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("accountclass_pkey", x => x.id);
                    table.ForeignKey(
                        name: "accountclass_sourceid_fkey",
                        column: x => x.SourceId,
                        principalTable: "accountsource",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "account",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    accountcode = table.Column<int>(type: "integer", nullable: false),
                    classid = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("account_pkey", x => x.id);
                    table.ForeignKey(
                        name: "account_classid_fkey",
                        column: x => x.classid,
                        principalTable: "accountclass",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "element",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    accountid = table.Column<int>(type: "integer", nullable: false),
                    elementtypeid = table.Column<int>(type: "integer", nullable: false),
                    value = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("element_pkey", x => x.id);
                    table.ForeignKey(
                        name: "element_accountid_fkey",
                        column: x => x.accountid,
                        principalTable: "account",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "element_elementtypeid_fkey",
                        column: x => x.elementtypeid,
                        principalTable: "det",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "det",
                columns: new[] { "id", "description", "name" },
                values: new object[,]
                {
                    { 1, "Входящее сальдо актив", "IN_BALANCE_A" },
                    { 2, "Входящее сальдо пассив", "IN_BALANCE_P" },
                    { 3, "Обороты дебет", "TURNOVER_D" },
                    { 4, "Обороты кредит", "TURNOVER_K" },
                    { 5, "Исходящее сальдо актив", "OUT_BALANCE_A" },
                    { 6, "Исходящее сальдо пассив", "OUT_BALANCE_P" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_account_classid",
                table: "account",
                column: "classid");

            migrationBuilder.CreateIndex(
                name: "IX_accountclass_SourceId",
                table: "accountclass",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_element_accountid",
                table: "element",
                column: "accountid");

            migrationBuilder.CreateIndex(
                name: "IX_element_elementtypeid",
                table: "element",
                column: "elementtypeid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "element");

            migrationBuilder.DropTable(
                name: "account");

            migrationBuilder.DropTable(
                name: "det");

            migrationBuilder.DropTable(
                name: "accountclass");

            migrationBuilder.DropTable(
                name: "accountsource");
        }
    }
}
