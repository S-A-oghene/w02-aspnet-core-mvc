using EseogheneMovies.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EseogheneMovies.Migrations;

[DbContext(typeof(MovieDbContext))]
public partial class MovieDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
        modelBuilder.HasAnnotation("ProductVersion", "10.0.15");
        modelBuilder.HasAnnotation("Relational:MaxIdentifierLength", 64);

        modelBuilder.Entity("EseogheneMovies.Models.Movie", b =>
        {
            b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("INTEGER");
            b.Property<string>("Genre").IsRequired().HasMaxLength(30).HasColumnType("TEXT");
            b.Property<decimal>("Price").HasColumnType("TEXT");
            b.Property<string>("Rating").IsRequired().HasMaxLength(5).HasColumnType("TEXT");
            b.Property<DateTime>("ReleaseDate").HasColumnType("TEXT");
            b.Property<string>("Title").IsRequired().HasMaxLength(60).HasColumnType("TEXT");
            b.HasKey("Id");
            b.ToTable("Movies");
        });
    }
}
