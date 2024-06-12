﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ODT_System.Models;

public partial class OdtsystemContext : DbContext
{
    public OdtsystemContext()
    {
    }

    public OdtsystemContext(DbContextOptions<OdtsystemContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Chat> Chats { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<StudyTime> StudyTimes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Chat>(entity =>
        {
            entity.ToTable("Chat");

            entity.Property(e => e.Content).HasColumnType("ntext");
            entity.Property(e => e.Time).HasColumnType("datetime");

            entity.HasOne(d => d.FromNavigation).WithMany(p => p.ChatFromNavigations)
                .HasForeignKey(d => d.From)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Chat_User");

            entity.HasOne(d => d.ToNavigation).WithMany(p => p.ChatToNavigations)
                .HasForeignKey(d => d.To)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Chat_User1");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.ToTable("Post");

            entity.Property(e => e.ContactPhone)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Description).HasColumnType("ntext");
            entity.Property(e => e.Fee).HasColumnType("money");
            entity.Property(e => e.ShortDescription).HasColumnType("ntext");
            entity.Property(e => e.StudentGender).HasDefaultValue((byte)3);
            entity.Property(e => e.StudyAddress).HasColumnType("ntext");
            entity.Property(e => e.StudyHour).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Subject).HasMaxLength(255);

            entity.HasOne(d => d.User).WithMany(p => p.Posts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Post_User");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<StudyTime>(entity =>
        {
            entity.ToTable("StudyTime");

            entity.HasOne(d => d.Post).WithMany(p => p.StudyTimes)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StudyTime_Post");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.Desciption).HasColumnType("ntext");
            entity.Property(e => e.Dob).HasColumnName("DOB");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FullName).HasMaxLength(255);
            entity.Property(e => e.Password).IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
