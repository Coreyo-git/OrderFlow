using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CustomerAddresses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EmailLastChangedAtUtc",
                table: "customers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "billing_city",
                table: "customers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "billing_country",
                table: "customers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "billing_postal_code",
                table: "customers",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "billing_state",
                table: "customers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "billing_street",
                table: "customers",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "business_address_id",
                table: "customers",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "shipping_address_id",
                table: "customers",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "shipping_city",
                table: "customers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "shipping_country",
                table: "customers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "shipping_postal_code",
                table: "customers",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "shipping_state",
                table: "customers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "shipping_street",
                table: "customers",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_customers_is_active",
                table: "customers",
                column: "is_active");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_customers_is_active",
                table: "customers");

            migrationBuilder.DropColumn(
                name: "EmailLastChangedAtUtc",
                table: "customers");

            migrationBuilder.DropColumn(
                name: "billing_city",
                table: "customers");

            migrationBuilder.DropColumn(
                name: "billing_country",
                table: "customers");

            migrationBuilder.DropColumn(
                name: "billing_postal_code",
                table: "customers");

            migrationBuilder.DropColumn(
                name: "billing_state",
                table: "customers");

            migrationBuilder.DropColumn(
                name: "billing_street",
                table: "customers");

            migrationBuilder.DropColumn(
                name: "business_address_id",
                table: "customers");

            migrationBuilder.DropColumn(
                name: "shipping_address_id",
                table: "customers");

            migrationBuilder.DropColumn(
                name: "shipping_city",
                table: "customers");

            migrationBuilder.DropColumn(
                name: "shipping_country",
                table: "customers");

            migrationBuilder.DropColumn(
                name: "shipping_postal_code",
                table: "customers");

            migrationBuilder.DropColumn(
                name: "shipping_state",
                table: "customers");

            migrationBuilder.DropColumn(
                name: "shipping_street",
                table: "customers");
        }
    }
}
