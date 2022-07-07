using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace BackEndBDAPP.Models
{
    public partial class UniKPContext : DbContext
    {
        public UniKPContext()
        {
        }

        public UniKPContext(DbContextOptions<UniKPContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ComfirmCode> ComfirmCodes { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Follow> Follows { get; set; }
        public virtual DbSet<Like> Likes { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<Tag1> Tags1 { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=159.65.94.122;User ID=sa;Password=beLeaf1999uni;Database=UniKP;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=True;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<ComfirmCode>(entity =>
            {
                entity.ToTable("ComfirmCode");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.DeleteDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Delete_Date")
                    .HasDefaultValueSql("(dateadd(minute,(10),getdate()))");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.HasOne(d => d.UsernameNavigation)
                    .WithMany(p => p.ComfirmCodes)
                    .HasForeignKey(d => d.Username)
                    .HasConstraintName("FK_ComfirmCode_ToUser");
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasKey(e => e.IdComment);

                entity.Property(e => e.IdComment).HasColumnName("id_Comment");

                entity.Property(e => e.CommentText)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.IdPost).HasColumnName("Id_Post");

                entity.Property(e => e.IdUser)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("Id_User");

                entity.HasOne(d => d.IdPostNavigation)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.IdPost)
                    .HasConstraintName("FK_Comments_ToPost");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Comments_ToUser");
            });

            modelBuilder.Entity<Follow>(entity =>
            {
                entity.HasKey(e => e.IdFollow);

                entity.ToTable("Follow");

                entity.Property(e => e.IdFollow).HasColumnName("Id_Follow");

                entity.Property(e => e.IdFollowed)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("Id_Followed");

                entity.Property(e => e.IdFollower)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("Id_Follower");

                entity.HasOne(d => d.IdFollowedNavigation)
                    .WithMany(p => p.FollowIdFollowedNavigations)
                    .HasForeignKey(d => d.IdFollowed)
                    .HasConstraintName("FK_Follow_ToFollowed");

                entity.HasOne(d => d.IdFollowerNavigation)
                    .WithMany(p => p.FollowIdFollowerNavigations)
                    .HasForeignKey(d => d.IdFollower)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Follow_ToFollower");
            });

            modelBuilder.Entity<Like>(entity =>
            {
                entity.HasKey(e => e.IdLike);

                entity.Property(e => e.IdLike).HasColumnName("Id_Like");

                entity.Property(e => e.IdPost).HasColumnName("Id_Post");

                entity.Property(e => e.IdUser)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("Id_User");

                entity.HasOne(d => d.IdPostNavigation)
                    .WithMany(p => p.Likes)
                    .HasForeignKey(d => d.IdPost)
                    .HasConstraintName("FK_Likes_ToPost");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Likes)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Likes_ToUser");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.HasKey(e => e.IdPost);

                entity.ToTable("Post");

                entity.Property(e => e.IdPost).HasColumnName("Id_Post");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Create_Date");

                entity.Property(e => e.DeleteDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Delete_Date");

                entity.Property(e => e.Description).HasMaxLength(250);

                entity.Property(e => e.IdUser)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("Id_User");

                entity.Property(e => e.Photo)
                    .IsRequired()
                    .HasColumnType("image");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.IdUser)
                    .HasConstraintName("FK_Post_ToTable");
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.HasKey(e => e.TagName)
                    .HasName("PK__tmp_ms_x__BDE0FD1C5306F6F1");

                entity.ToTable("Tag");

                entity.Property(e => e.TagName).HasMaxLength(250);
            });

            modelBuilder.Entity<Tag1>(entity =>
            {
                entity.HasKey(e => e.IdTagsConnrction);

                entity.ToTable("Tags");

                entity.Property(e => e.IdTagsConnrction).HasColumnName("Id_TagsConnrction");

                entity.Property(e => e.IdPost).HasColumnName("Id_Post");

                entity.Property(e => e.IdTag)
                    .IsRequired()
                    .HasMaxLength(250)
                    .HasColumnName("Id_Tag");

                entity.HasOne(d => d.IdPostNavigation)
                    .WithMany(p => p.Tag1s)
                    .HasForeignKey(d => d.IdPost)
                    .HasConstraintName("FK_Tags_Post");

                entity.HasOne(d => d.IdTagNavigation)
                    .WithMany(p => p.Tag1s)
                    .HasForeignKey(d => d.IdTag)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Tags_Tag");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Username);

                entity.ToTable("User");

                entity.HasIndex(e => e.Email, "AK_User_Email")
                    .IsUnique();

                entity.HasIndex(e => e.Username, "AK_User_Username")
                    .IsUnique();

                entity.Property(e => e.Username)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Description).HasMaxLength(250);

                entity.Property(e => e.Email)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.EmailConfirm).HasDefaultValueSql("((0))");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("First_Name");

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Last_Name");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Photo).HasColumnType("image");

                entity.Property(e => e.RefreshToken).HasColumnName("Refresh_Token");

                entity.Property(e => e.RefreshTokenExpireTime).HasColumnName("Refresh_Token_Expire_Time");

                entity.Property(e => e.Salt).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
