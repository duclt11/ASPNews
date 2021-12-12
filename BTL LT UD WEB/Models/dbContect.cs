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
        public virtual DbSet<poster> posters { get; set; }
        public virtual DbSet<post> posts { get; set; }
        public virtual DbSet<user> users { get; set; }
        public virtual DbSet<comment> comments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<admin>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<admin>()
                .Property(e => e.password)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<admin>()
                .Property(e => e.reset_password)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<admin>()
                .Property(e => e.avatar)
                .IsUnicode(false);

            modelBuilder.Entity<poster>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<poster>()
                .Property(e => e.password)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<poster>()
                .Property(e => e.avatar)
                .IsUnicode(false);

            modelBuilder.Entity<post>()
                .Property(e => e.avatar)
                .IsUnicode(false);

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
