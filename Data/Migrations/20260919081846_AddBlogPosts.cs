using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarWashWebsite.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddBlogPosts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlogPosts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TitleDe = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    TitleEn = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    SlugDe = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    SlugEn = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    SummaryDe = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    SummaryEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ContentDe = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContentEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CoverImagePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ReadingTimeMinutes = table.Column<int>(type: "int", nullable: false),
                    SeoTitleDe = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    SeoTitleEn = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    MetaDescriptionDe = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    MetaDescriptionEn = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false),
                    PublishedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ViewCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogPosts", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlogPosts_Category",
                table: "BlogPosts",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_BlogPosts_IsPublished",
                table: "BlogPosts",
                column: "IsPublished");

            migrationBuilder.CreateIndex(
                name: "IX_BlogPosts_PublishedAt",
                table: "BlogPosts",
                column: "PublishedAt");

            migrationBuilder.CreateIndex(
                name: "IX_BlogPosts_SlugDe",
                table: "BlogPosts",
                column: "SlugDe",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BlogPosts_SlugEn",
                table: "BlogPosts",
                column: "SlugEn",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogPosts");
        }
    }
}
