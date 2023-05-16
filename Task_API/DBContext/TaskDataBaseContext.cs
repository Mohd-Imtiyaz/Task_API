using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Task_API.Model;

namespace Task_API.DBContext;

public partial class TaskDataBaseContext : DbContext
{
    public TaskDataBaseContext()
    {
    }

    public TaskDataBaseContext(DbContextOptions<TaskDataBaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TUser> TUsers { get; set; }

    public virtual DbSet<TUserTask> TUserTasks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-FTFDBLV;Initial Catalog=Task_DataBase;Persist Security Info=True;User ID=sa;Password=\"1234\";TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TUser>(entity =>
        {
            entity.HasKey(e => e.UId).HasName("PK__T_Users__5A2040DB33DDE484");

            entity.ToTable("T_Users");

            entity.HasIndex(e => e.UUserName, "UQ__T_Users__A36AF9B0FE43D9AF").IsUnique();

            entity.Property(e => e.UId).HasColumnName("U_ID");
            entity.Property(e => e.ActiveStatus)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("Active_Status");
            entity.Property(e => e.Roles)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UEmail)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("U_Email");
            entity.Property(e => e.UName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("U_Name");
            entity.Property(e => e.UPassword)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("U_Password");
            entity.Property(e => e.UUserName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("U_User_Name");
        });

        modelBuilder.Entity<TUserTask>(entity =>
        {
            entity.HasKey(e => e.TId).HasName("PK__T_User_T__83BB1FB26EF6D069");

            entity.ToTable("T_User_Tasks");

            entity.Property(e => e.TId).HasColumnName("T_ID");
            entity.Property(e => e.TDescription).HasColumnName("T_Description");
            entity.Property(e => e.TEndDate)
                .HasColumnType("date")
                .HasColumnName("T_End_Date");
            entity.Property(e => e.TFile).HasColumnName("T_File");
            entity.Property(e => e.TStartDate)
                .HasColumnType("date")
                .HasColumnName("T_Start_Date");
            entity.Property(e => e.TTaskCreater)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("T_Task_Creater");
            entity.Property(e => e.TTitle)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("T_Title");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
