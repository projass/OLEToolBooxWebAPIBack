using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace OLEToolBoxWebAPIPruebas.Models;

public partial class PruebasoletoolboxContext : DbContext
{
    public PruebasoletoolboxContext()
    {
    }

    public PruebasoletoolboxContext(DbContextOptions<PruebasoletoolboxContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<CompanyService> CompanyServices { get; set; }

    public virtual DbSet<Family> Families { get; set; }

    public virtual DbSet<MainCompany> MainCompanies { get; set; }

    public virtual DbSet<MessagesForum> MessagesForums { get; set; }

    public virtual DbSet<Operacione> Operaciones { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SystemDatum> SystemData { get; set; }

    public virtual DbSet<TopicsForum> TopicsForums { get; set; }

    public virtual DbSet<UserCredential> UserCredentials { get; set; }

    public virtual DbSet<UserProfile> UserProfiles { get; set; }

    public virtual DbSet<UsersDatum> UsersData { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=pruebasoletoolbox;Integrated Security=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.CompanyId).HasName("PK__Companie__2D971CACF839FF06");

            entity.Property(e => e.Comments)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.CompanyComCode).HasMaxLength(40);
            entity.Property(e => e.CompanyInfo)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.CompanyName).HasMaxLength(50);
            entity.Property(e => e.DateFound).HasColumnType("datetime");
        });

        modelBuilder.Entity<CompanyService>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PK__CompanyS__C51BB0EA18ADFEBF");

            entity.Property(e => e.ServiceId).HasColumnName("ServiceID");
            entity.Property(e => e.Comments)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ServiceCode).HasMaxLength(200);
            entity.Property(e => e.ServiceDescription)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.ServiceName).HasMaxLength(50);
        });

        modelBuilder.Entity<Family>(entity =>
        {
            entity.HasKey(e => e.IdFamily).HasName("PK__Families__E203C3A84D7E1B89");

            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<MainCompany>(entity =>
        {
            entity.HasKey(e => e.MainId).HasName("PK__MainComp__71F8F5CC3591B812");

            entity.ToTable("MainCompany");

            entity.Property(e => e.MainId).HasMaxLength(50);
            entity.Property(e => e.DateFund).HasColumnType("datetime");
            entity.Property(e => e.MainCompanyName).HasMaxLength(100);
            entity.Property(e => e.Sector)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Size).HasMaxLength(50);
        });

        modelBuilder.Entity<MessagesForum>(entity =>
        {
            entity.HasKey(e => e.IdMessage).HasName("PK__Messages__47AAF304E0BCB322");

            entity.ToTable("MessagesForum");

            entity.Property(e => e.AliasMessage).HasMaxLength(30);
            entity.Property(e => e.DateMessage).HasColumnType("datetime");

            entity.HasOne(d => d.IdTopicNavigation).WithMany(p => p.MessagesForums)
                .HasForeignKey(d => d.IdTopic)
                .HasConstraintName("FK__MessagesF__IdTop__4E88ABD4");

            entity.HasOne(d => d.IdUserMessageNavigation).WithMany(p => p.MessagesForums)
                .HasForeignKey(d => d.IdUserMessage)
                .HasConstraintName("FK__MessagesF__IdUse__4F7CD00D");
        });

        modelBuilder.Entity<Operacione>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Operacio__3214EC07C0271A8B");

            entity.Property(e => e.Controller).HasMaxLength(50);
            entity.Property(e => e.FechaAccion).HasColumnType("datetime");
            entity.Property(e => e.Ip).HasMaxLength(50);
            entity.Property(e => e.Operacion).HasMaxLength(50);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.IdProduct).HasName("PK__Products__2E8946D4F93426F0");

            entity.Property(e => e.Name).HasMaxLength(150);
            entity.Property(e => e.PictureUrl).HasColumnName("PictureURL");
            entity.Property(e => e.Price).HasColumnType("decimal(9, 2)");

            entity.HasOne(d => d.Family).WithMany(p => p.Products)
                .HasForeignKey(d => d.FamilyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Familias_Productos");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE1AE19D8D9C");

            entity.Property(e => e.RoleDescription).HasMaxLength(30);
        });

        modelBuilder.Entity<SystemDatum>(entity =>
        {
            entity.HasKey(e => e.SystemId).HasName("PK__SystemDa__9394F68AAD103A72");

            entity.Property(e => e.DateNow).HasColumnType("datetime");
            entity.Property(e => e.DateStart).HasColumnType("datetime");
            entity.Property(e => e.SystemName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.SystemVersion).HasMaxLength(30);
            entity.Property(e => e.TotalUsers).HasDefaultValue(0);
        });

        modelBuilder.Entity<TopicsForum>(entity =>
        {
            entity.HasKey(e => e.IdTopic).HasName("PK__TopicsFo__C34052838393DC5F");

            entity.ToTable("TopicsForum");

            entity.Property(e => e.DateCreated).HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(60);

            entity.HasOne(d => d.IdUserTopicNavigation).WithMany(p => p.TopicsForums)
                .HasForeignKey(d => d.IdUserTopic)
                .HasConstraintName("FK__TopicsFor__IdUse__4BAC3F29");
        });

        modelBuilder.Entity<UserCredential>(entity =>
        {
            entity.HasKey(e => e.CredId).HasName("PK__UserCred__9417225D21B146F1");

            entity.Property(e => e.ChangePasswordLink).HasMaxLength(50);
            entity.Property(e => e.CredPassword).HasMaxLength(500);
            entity.Property(e => e.DateLink).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
        });

        modelBuilder.Entity<UserProfile>(entity =>
        {
            entity.HasKey(e => e.ProfileId).HasName("PK__UserProf__290C88E4F7AA93DF");

            entity.Property(e => e.Birthday).HasColumnType("datetime");
            entity.Property(e => e.PhoneNumber).HasMaxLength(100);
            entity.Property(e => e.ProfileAlias)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ProfileApel)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ProfileName)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Credentials).WithMany(p => p.UserProfiles)
                .HasForeignKey(d => d.CredentialsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Credentials_Profiles");

            entity.HasOne(d => d.ProfileRoleNavigation).WithMany(p => p.UserProfiles)
                .HasForeignKey(d => d.ProfileRole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Roles_Profiles");
        });

        modelBuilder.Entity<UsersDatum>(entity =>
        {
            entity.HasKey(e => e.DataId).HasName("PK__UsersDat__9D05303DE9571F69");

            entity.HasOne(d => d.Cred).WithMany(p => p.UsersData)
                .HasForeignKey(d => d.CredId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Credentials_Data");

            entity.HasOne(d => d.Profile).WithMany(p => p.UsersData)
                .HasForeignKey(d => d.ProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Profiles_Data");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
