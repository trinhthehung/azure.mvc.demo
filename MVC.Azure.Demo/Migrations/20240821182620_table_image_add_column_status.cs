using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVC.Azure.Demo.Migrations
{
    /// <inheritdoc />
    public partial class table_image_add_column_status : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Image",
                type: "int",
                nullable: false,
                defaultValue: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Image");
        }
    }
}
