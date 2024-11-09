using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVC.Azure.Demo.Migrations
{
    /// <inheritdoc />
    public partial class addCloumnBlogName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BlogName",
                table: "ImageEx",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BlogName",
                table: "Image",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BlogName",
                table: "ImageEx");

            migrationBuilder.DropColumn(
                name: "BlogName",
                table: "Image");
        }
    }
}
