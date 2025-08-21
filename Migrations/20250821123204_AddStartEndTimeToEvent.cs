using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventEase.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddStartEndTimeToEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "EndTime",
                table: "Events",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "StartTime",
                table: "Events",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AlterColumn<DateTime>(
                name: "BookingDate",
                table: "Bookings",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "EndTime",
                table: "Bookings",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "StartTime",
                table: "Bookings",
                type: "time",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "Bookings");

            migrationBuilder.AlterColumn<DateTime>(
                name: "BookingDate",
                table: "Bookings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
