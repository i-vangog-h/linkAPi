using Microsoft.EntityFrameworkCore;
using linkApi.Entities;

namespace linkApi.DataContext;

public partial class LinkShortenerContext : DbContext
{
    public LinkShortenerContext()
    {
    }

    public LinkShortenerContext(DbContextOptions<LinkShortenerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Url> Urls { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=link_shortener;Username=postgres;Password=12345678"); 
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
