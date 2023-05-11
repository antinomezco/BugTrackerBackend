using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BugTracker.Migrations
{
    /// <inheritdoc />
    public partial class TicketRemoveSubmitterPersonId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_AspNetUsers_SubmitterId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Personnel_SubmitterPersonId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_SubmitterId",
                table: "Tickets");

            migrationBuilder.AlterColumn<int>(
                name: "SubmitterPersonId",
                table: "Tickets",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "SubmitterId",
                table: "Tickets",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubmitterId1",
                table: "Tickets",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_SubmitterId1",
                table: "Tickets",
                column: "SubmitterId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_AspNetUsers_SubmitterId1",
                table: "Tickets",
                column: "SubmitterId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Personnel_SubmitterPersonId",
                table: "Tickets",
                column: "SubmitterPersonId",
                principalTable: "Personnel",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_AspNetUsers_SubmitterId1",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Personnel_SubmitterPersonId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_SubmitterId1",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "SubmitterId1",
                table: "Tickets");

            migrationBuilder.AlterColumn<int>(
                name: "SubmitterPersonId",
                table: "Tickets",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SubmitterId",
                table: "Tickets",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_SubmitterId",
                table: "Tickets",
                column: "SubmitterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_AspNetUsers_SubmitterId",
                table: "Tickets",
                column: "SubmitterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Personnel_SubmitterPersonId",
                table: "Tickets",
                column: "SubmitterPersonId",
                principalTable: "Personnel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
