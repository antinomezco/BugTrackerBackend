using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BugTracker.Migrations
{
    /// <inheritdoc />
    public partial class ChatgptSuggestions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Personnel_PersonAssignedId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Personnel_PersonSubmitterId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_PersonAssignedId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_PersonSubmitterId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "PersonAssignedId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "PersonSubmitterId",
                table: "Tickets");

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Projects",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "TicketsPersonnel",
                columns: table => new
                {
                    TicketId = table.Column<int>(type: "int", nullable: false),
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    isSubmitter = table.Column<bool>(type: "bit", nullable: false),
                    PersonId1 = table.Column<int>(type: "int", nullable: true),
                    TicketId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketsPersonnel", x => new { x.PersonId, x.TicketId });
                    table.ForeignKey(
                        name: "FK_TicketsPersonnel_Personnel_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Personnel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketsPersonnel_Personnel_PersonId1",
                        column: x => x.PersonId1,
                        principalTable: "Personnel",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TicketsPersonnel_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketsPersonnel_Tickets_TicketId1",
                        column: x => x.TicketId1,
                        principalTable: "Tickets",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TicketsPersonnel_PersonId1",
                table: "TicketsPersonnel",
                column: "PersonId1");

            migrationBuilder.CreateIndex(
                name: "IX_TicketsPersonnel_TicketId",
                table: "TicketsPersonnel",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketsPersonnel_TicketId1",
                table: "TicketsPersonnel",
                column: "TicketId1",
                unique: true,
                filter: "[TicketId1] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TicketsPersonnel");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Projects");

            migrationBuilder.AddColumn<int>(
                name: "PersonAssignedId",
                table: "Tickets",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PersonSubmitterId",
                table: "Tickets",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_PersonAssignedId",
                table: "Tickets",
                column: "PersonAssignedId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_PersonSubmitterId",
                table: "Tickets",
                column: "PersonSubmitterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Personnel_PersonAssignedId",
                table: "Tickets",
                column: "PersonAssignedId",
                principalTable: "Personnel",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Personnel_PersonSubmitterId",
                table: "Tickets",
                column: "PersonSubmitterId",
                principalTable: "Personnel",
                principalColumn: "Id");
        }
    }
}
