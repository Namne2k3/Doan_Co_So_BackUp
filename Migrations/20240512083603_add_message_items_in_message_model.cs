using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Doan_Web_CK.Migrations
{
    /// <inheritdoc />
    public partial class add_message_items_in_message_model : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MessageImages",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MessageImages",
                table: "Messages");
        }
    }
}
