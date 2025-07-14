using System;
using System.Collections.Generic;
using EfSqlPerformance.Api.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace EfSqlPerformance.Api.Data;

public partial class StackOverflowContext : DbContext
{
    public StackOverflowContext(DbContextOptions<StackOverflowContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Badges> Badges { get; set; }

    public virtual DbSet<Comments> Comments { get; set; }

    public virtual DbSet<LinkTypes> LinkTypes { get; set; }

    public virtual DbSet<PostLinks> PostLinks { get; set; }

    public virtual DbSet<PostTypes> PostTypes { get; set; }

    public virtual DbSet<Posts> Posts { get; set; }

    public virtual DbSet<Users> Users { get; set; }

    public virtual DbSet<VoteTypes> VoteTypes { get; set; }

    public virtual DbSet<Votes> Votes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Badges>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Badges__Id");

            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(40);
        });

        modelBuilder.Entity<Comments>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Comments__Id");

            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.Text).HasMaxLength(700);
        });

        modelBuilder.Entity<LinkTypes>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_LinkTypes__Id");

            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PostLinks>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_PostLinks__Id");

            entity.Property(e => e.CreationDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<PostTypes>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_PostTypes__Id");

            entity.Property(e => e.Type).HasMaxLength(50);
        });

        modelBuilder.Entity<Posts>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Posts__Id");

            entity.Property(e => e.ClosedDate).HasColumnType("datetime");
            entity.Property(e => e.CommunityOwnedDate).HasColumnType("datetime");
            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.LastActivityDate).HasColumnType("datetime");
            entity.Property(e => e.LastEditDate).HasColumnType("datetime");
            entity.Property(e => e.LastEditorDisplayName).HasMaxLength(40);
            entity.Property(e => e.Tags).HasMaxLength(150);
            entity.Property(e => e.Title).HasMaxLength(250);
        });

        modelBuilder.Entity<Users>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Users_Id");

            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.DisplayName).HasMaxLength(40);
            entity.Property(e => e.EmailHash).HasMaxLength(40);
            entity.Property(e => e.LastAccessDate).HasColumnType("datetime");
            entity.Property(e => e.Location).HasMaxLength(100);
            entity.Property(e => e.WebsiteUrl).HasMaxLength(200);
        });

        modelBuilder.Entity<VoteTypes>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_VoteType__Id");

            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Votes>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Votes__Id");

            entity.Property(e => e.CreationDate).HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
