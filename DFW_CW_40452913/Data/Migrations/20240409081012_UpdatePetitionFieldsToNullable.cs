using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DFW_CW_40452913.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePetitionFieldsToNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "Petition");

            migrationBuilder.AlterColumn<int>(
                name: "Votes",
                table: "Petition",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Votes",
                table: "Petition",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "Petition",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}
