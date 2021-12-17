using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace BTL_LT_UD_WEB.Models
{
    public partial class dbContect : DbContext
    {
        public dbContect()
            : base("name=dbContect")
        {
        }

        public virtual DbSet<admin> admins { get; set; }
        public virtual DbSet<category> categories { get; set; }
        public virtual DbSet<comment> comments { get; set; }
        public virtual DbSet<poster> posters { get; set; }
        public virtual DbSet<post> posts { get; set; }
        public virtual DbSet<user> users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<admin>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<admin>()
                .Property(e => e.password)
                .IsUnicode(false);

            modelBuilder.Entity<admin>()
                .Property(e => e.avatar)
                .IsUnicode(false);

            modelBuilder.Entity<category>()
                .HasMany(e => e.posts)
                .WithOptional(e => e.category)
                .HasForeignKey(e => e.category_id);

            modelBuilder.Entity<category>()
                .HasMany(e => e.posts1)
                .WithOptional(e => e.category1)
                .HasForeignKey(e => e.category_id);

            modelBuilder.Entity<poster>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<poster>()
                .Property(e => e.password)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<poster>()
                .HasMany(e => e.posts)
                .WithOptional(e => e.poster)
                .HasForeignKey(e => e.poster_id);

            modelBuilder.Entity<poster>()
                .HasMany(e => e.posts1)
                .WithOptional(e => e.poster1)
                .HasForeignKey(e => e.poster_id);

            modelBuilder.Entity<user>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.password)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.avatar)
                .IsUnicode(false);
        }
    }
}
