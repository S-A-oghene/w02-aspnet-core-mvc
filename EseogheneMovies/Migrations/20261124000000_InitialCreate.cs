using EseogheneMovies.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EseogheneMovies.Migrations;

[DbContext(typeof(MovieDbContext))]
[Migration("20261124000000_InitialCreate")]
public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Movies",
            columns: table => new
            {
                Id = table.Column<int>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                Title = table.Column<string>(type: "TEXT", maxLength: 60, nullable: false),
                ReleaseDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                Genre = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                Price = table.Column<decimal>(type: "TEXT", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Movies", x => x.Id);
            });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "Movies");
    }

    protected override void BuildTargetModel(ModelBuilder modelBuilder)
    {
        modelBuilder.HasAnnotation("ProductVersion", "10.0.15");
        modelBuilder.HasAnnotation("Relational:MaxIdentifierLength", 64);

        modelBuilder.Entity("EseogheneMovies.Models.Movie", b =>
        {
            b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("INTEGER");
            b.Property<string>("Genre").IsRequired().HasMaxLength(30).HasColumnType("TEXT");
            b.Property<decimal>("Price").HasColumnType("TEXT");
            b.Property<DateTime>("ReleaseDate").HasColumnType("TEXT");
            b.Property<string>("Title").IsRequired().HasMaxLength(60).HasColumnType("TEXT");
            b.HasKey("Id");
            b.ToTable("Movies");
        });
    }
}
