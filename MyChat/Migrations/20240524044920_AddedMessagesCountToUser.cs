using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyChat.Migrations
{
    /// <inheritdoc />
    public partial class AddedMessagesCountToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MessagesCount",
                table: "AspNetUsers",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MessagesCount",
                table: "AspNetUsers");
        }
    }
}
