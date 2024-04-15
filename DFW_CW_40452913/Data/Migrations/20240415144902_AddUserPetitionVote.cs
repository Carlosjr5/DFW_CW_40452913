using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DFW_CW_40452913.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserPetitionVote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Petition_PetitionId",
                table: "Comment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comment",
                table: "Comment");

            migrationBuilder.RenameTable(
                name: "Comment",
                newName: "Comments");

            migrationBuilder.RenameIndex(
                name: "IX_Comment_PetitionId",
                table: "Comments",
                newName: "IX_Comments_PetitionId");

            migrationBuilder.AddColumn<int>(
                name: "Votes",
                table: "Petition",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comments",
                table: "Comments",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "UserVotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PetitionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserVotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserVotes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserVotes_Petition_PetitionId",
                        column: x => x.PetitionId,
                        principalTable: "Petition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserVotes_PetitionId",
                table: "UserVotes",
                column: "PetitionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserVotes_UserId",
                table: "UserVotes",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Petition_PetitionId",
                table: "Comments",
                column: "PetitionId",
                principalTable: "Petition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Petition_PetitionId",
                table: "Comments");

            migrationBuilder.DropTable(
                name: "UserVotes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comments",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "Votes",
                table: "Petition");

            migrationBuilder.RenameTable(
                name: "Comments",
                newName: "Comment");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_PetitionId",
                table: "Comment",
                newName: "IX_Comment_PetitionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comment",
                table: "Comment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Petition_PetitionId",
                table: "Comment",
                column: "PetitionId",
                principalTable: "Petition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
