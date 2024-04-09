using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DFW_CW_40452913.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePetitionWithImageUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Petition",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Petition");
        }
    }
}
