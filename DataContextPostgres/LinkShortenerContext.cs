using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using linkApi.Entities;

namespace linkApi.DataContext;

public partial class LinkShortenerContext : DbContext
{
    private readonly IConfiguration _configuration;

    public LinkShortenerContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public LinkShortenerContext(DbContextOptions<LinkShortenerContext> options, IConfiguration configuration)
        : base(options)
    { 
        _configuration = configuration;
    }

    public virtual DbSet<Url> Urls { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql(_configuration.GetConnectionString("PostgresConnection"));
        }
 
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("en_US.UTF-8");

        modelBuilder.Entity<Url>(entity =>
        {
            entity.HasKey(e => e.HashKey).HasName("url_pkey");

            entity.ToTable("URL");

            entity.Property(e => e.HashKey)
                .HasMaxLength(100)
                .HasColumnName("hash_key");
            entity.Property(e => e.AccessCount)
                .HasDefaultValue(0)
                .HasColumnName("access_count");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.OriginalUrl).HasColumnName("original_url");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
