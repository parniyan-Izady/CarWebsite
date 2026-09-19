using CarWashWebsite.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CarWashWebsite.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Service> Services => Set<Service>();
    public DbSet<GalleryItem> GalleryItems => Set<GalleryItem>();
    public DbSet<FaqItem> FaqItems => Set<FaqItem>();
    public DbSet<ContactMessage> ContactMessages => Set<ContactMessage>();
    public DbSet<SiteSetting> SiteSettings => Set<SiteSetting>();
    public DbSet<PageContent> PageContents => Set<PageContent>();
    public DbSet<BlogPost> BlogPosts => Set<BlogPost>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // BlogPost Slugs and Indexes
        builder.Entity<BlogPost>(entity =>
        {
            entity.HasIndex(b => b.SlugDe).IsUnique();
            entity.HasIndex(b => b.SlugEn).IsUnique();
            entity.HasIndex(b => b.IsPublished);
            entity.HasIndex(b => b.Category);
            entity.HasIndex(b => b.PublishedAt);
        });

        // Service SEO Slugs uniqueness & indexing
        builder.Entity<Service>(entity =>
        {
            entity.HasIndex(s => s.SlugDe).IsUnique();
            entity.HasIndex(s => s.SlugEn).IsUnique();
            entity.HasIndex(s => s.IsActive);
            entity.HasIndex(s => s.DisplayOrder);
        });

        // Site Settings Key uniqueness
        builder.Entity<SiteSetting>(entity =>
        {
            entity.HasIndex(s => s.Key).IsUnique();
        });

        // PageContent Composite Index
        builder.Entity<PageContent>(entity =>
        {
            entity.HasIndex(p => new { p.PageKey, p.SectionKey }).IsUnique();
        });

        // ContactMessage Indexing
        builder.Entity<ContactMessage>(entity =>
        {
            entity.HasIndex(m => m.IsRead);
            entity.HasIndex(m => m.CreatedAt);
        });

        // Gallery Item Indexing
        builder.Entity<GalleryItem>(entity =>
        {
            entity.HasIndex(g => g.Category);
            entity.HasIndex(g => g.IsActive);
            entity.HasIndex(g => g.DisplayOrder);
        });

        // Faq Item Indexing
        builder.Entity<FaqItem>(entity =>
        {
            entity.HasIndex(f => f.IsActive);
            entity.HasIndex(f => f.DisplayOrder);
        });
    }
}
