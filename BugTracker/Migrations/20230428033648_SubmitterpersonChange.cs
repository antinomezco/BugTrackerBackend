using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BugTracker.Migrations
{
    /// <inheritdoc />
    public partial class SubmitterpersonChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TicketsPersonnel_Tickets_TicketId1",
                table: "TicketsPersonnel");

            migrationBuilder.DropIndex(
                name: "IX_TicketsPersonnel_TicketId1",
                table: "TicketsPersonnel");

            migrationBuilder.DropColumn(
                name: "TicketId1",
                table: "TicketsPersonnel");

            migrationBuilder.DropColumn(
                name: "isSubmitter",
                table: "TicketsPersonnel");

            migrationBuilder.AddColumn<int>(
                name: "SubmitterPersonId",
                table: "Tickets",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubmitterPersonId",
                table: "Tickets");

            migrationBuilder.AddColumn<int>(
                name: "TicketId1",
                table: "TicketsPersonnel",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isSubmitter",
                table: "TicketsPersonnel",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_TicketsPersonnel_TicketId1",
                table: "TicketsPersonnel",
                column: "TicketId1",
                unique: true,
                filter: "[TicketId1] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketsPersonnel_Tickets_TicketId1",
                table: "TicketsPersonnel",
                column: "TicketId1",
                principalTable: "Tickets",
                principalColumn: "Id");
        }
    }
}
