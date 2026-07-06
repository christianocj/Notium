using System;
using KnowledgeHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeHub.Infra.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Note> Notes => Set<Note>();
    public DbSet<Tag> Tags => Set<Tag>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(u => u.Email).IsUnique();
            entity.Property(u => u.Name).HasMaxLength(150).IsRequired();
            entity.Property(u => u.Email).HasMaxLength(200).IsRequired();
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.Property(c => c.Name).HasMaxLength(100).IsRequired();

            entity.HasOne(c => c.User)
                  .WithMany(u => u.Categories)
                  .HasForeignKey(c => c.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.Property(t => t.Name).HasMaxLength(50).IsRequired();

            entity.HasOne(t => t.User)
                  .WithMany(u => u.Tags)
                  .HasForeignKey(t => t.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            // Evita tag duplicada com mesmo nome para o mesmo usuário
            entity.HasIndex(t => new { t.UserId, t.Name }).IsUnique();
        });

        modelBuilder.Entity<Note>(entity =>
        {
            entity.Property(n => n.Title).HasMaxLength(200).IsRequired();
            entity.Property(n => n.Content).IsRequired();

            // Índices para melhorar performance de consultas
            entity.HasIndex(n => n.Title);
            entity.HasIndex(n => n.IsArchived);
            entity.HasIndex(n => n.IsDeleted);
            entity.HasIndex(n => n.IsFavorite);

            entity.HasOne(n => n.User)
                  .WithMany(u => u.Notes)
                  .HasForeignKey(n => n.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(n => n.Category)
                  .WithMany(c => c.Notes)
                  .HasForeignKey(n => n.CategoryId)
                  .OnDelete(DeleteBehavior.Restrict); // não deixa apagar categoria com notas

            // N:N implícito — configurado explicitamente para evitar múltiplos caminhos de cascade no SQL Server
            entity.HasMany(n => n.Tags)
                  .WithMany(t => t.Notes)
                  .UsingEntity<Dictionary<string, object>>(
                      "NoteTags",
                      j => j.HasOne<Tag>().WithMany().HasForeignKey("TagsId").OnDelete(DeleteBehavior.NoAction),
                      j => j.HasOne<Note>().WithMany().HasForeignKey("NotesId").OnDelete(DeleteBehavior.Cascade),
                      j => j.ToTable("NoteTags")
                  );

            // Query filter global: nunca retorna notas com IsDeleted = true por padrão
            entity.HasQueryFilter(n => !n.IsDeleted);
        });
    }
}