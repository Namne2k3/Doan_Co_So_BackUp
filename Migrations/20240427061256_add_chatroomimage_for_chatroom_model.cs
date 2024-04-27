using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Doan_Web_CK.Migrations
{
    /// <inheritdoc />
    public partial class add_chatroomimage_for_chatroom_model : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChatRoomImage",
                table: "chatRooms",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChatRoomImage",
                table: "chatRooms");
        }
    }
}
