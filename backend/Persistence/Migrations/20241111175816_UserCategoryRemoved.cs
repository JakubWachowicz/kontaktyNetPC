using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UserCategoryRemoved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_ProfileCategory_ProfileCategoryId",
                table: "UserProfiles");

            migrationBuilder.DropTable(
                name: "ProfileSubCategory");

            migrationBuilder.DropTable(
                name: "ProfileCategory");

            migrationBuilder.DropIndex(
                name: "IX_UserProfiles_ProfileCategoryId",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "ProfileCategoryId",
                table: "UserProfiles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProfileCategoryId",
                table: "UserProfiles",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ProfileCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CategoryName = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProfileSubCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProfileCategoryId = table.Column<int>(type: "INTEGER", nullable: false),
                    SubcategoryName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileSubCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProfileSubCategory_ProfileCategory_ProfileCategoryId",
                        column: x => x.ProfileCategoryId,
                        principalTable: "ProfileCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_ProfileCategoryId",
                table: "UserProfiles",
                column: "ProfileCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileSubCategory_ProfileCategoryId",
                table: "ProfileSubCategory",
                column: "ProfileCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_ProfileCategory_ProfileCategoryId",
                table: "UserProfiles",
                column: "ProfileCategoryId",
                principalTable: "ProfileCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
