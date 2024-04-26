using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Doan_Web_CK.Migrations
{
    /// <inheritdoc />
    public partial class Modify_ChatRoom_Model : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_chatRooms_AspNetUsers_FriendId",
                table: "chatRooms");

            migrationBuilder.DropForeignKey(
                name: "FK_chatRooms_AspNetUsers_UserId",
                table: "chatRooms");

            migrationBuilder.DropIndex(
                name: "IX_chatRooms_FriendId",
                table: "chatRooms");

            migrationBuilder.DropIndex(
                name: "IX_chatRooms_UserId",
                table: "chatRooms");

            migrationBuilder.DropColumn(
                name: "FriendId",
                table: "chatRooms");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "chatRooms");

            migrationBuilder.CreateTable(
                name: "ApplicationUserChatRoom",
                columns: table => new
                {
                    ChatroomsId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserChatRoom", x => new { x.ChatroomsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserChatRoom_AspNetUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserChatRoom_chatRooms_ChatroomsId",
                        column: x => x.ChatroomsId,
                        principalTable: "chatRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserChatRoom_UsersId",
                table: "ApplicationUserChatRoom",
                column: "UsersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserChatRoom");

            migrationBuilder.AddColumn<string>(
                name: "FriendId",
                table: "chatRooms",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "chatRooms",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_chatRooms_FriendId",
                table: "chatRooms",
                column: "FriendId");

            migrationBuilder.CreateIndex(
                name: "IX_chatRooms_UserId",
                table: "chatRooms",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_chatRooms_AspNetUsers_FriendId",
                table: "chatRooms",
                column: "FriendId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_chatRooms_AspNetUsers_UserId",
                table: "chatRooms",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
