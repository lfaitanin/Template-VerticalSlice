using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedProfileData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Inserting initial data into tb_profile table
            migrationBuilder.InsertData(
                table: "tb_profile",
                columns: new[] { "description", "status" },
                values: new object[,]
                {
                    { "Manager", "Active" },
                    { "User", "Active" },
                    { "Administrator", "Active" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Removing inserted data
            migrationBuilder.DeleteData(
                table: "tb_profile",
                keyColumn: "description",
                keyValues: new object[] { "Manager" , "User", "Administrator" });
        }
    }
}
