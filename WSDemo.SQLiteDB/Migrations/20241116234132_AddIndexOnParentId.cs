using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WSDemo.SQLiteDB.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexOnParentId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_FolderItems_ParentId",
                table: "FolderItems",
                column: "ParentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FolderItems_ParentId",
                table: "FolderItems");
        }
    }
}
