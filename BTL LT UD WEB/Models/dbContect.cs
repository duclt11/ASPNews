using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace BTL_LT_UD_WEB.Models
{
    public partial class dbContect : DbContext
    {
        public dbContect()
            : base("name=dbContect1")
        {
        }

        public virtual DbSet<admins> admins { get; set; }
        public virtual DbSet<categories> categories { get; set; }
        public virtual DbSet<comments> comments { get; set; }
        public virtual DbSet<poster> poster { get; set; }
        public virtual DbSet<posts> posts { get; set; }
        public virtual DbSet<users> users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<admins>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<admins>()
                .Property(e => e.password)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<admins>()
                .Property(e => e.reset_password)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<admins>()
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
                .Property(e => e.reset_password)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<poster>()
                .Property(e => e.avatar)
                .IsUnicode(false);

            modelBuilder.Entity<posts>()
                .Property(e => e.avatar)
                .IsUnicode(false);

            modelBuilder.Entity<users>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<users>()
                .Property(e => e.password)
                .IsFixedLength()
                .IsUnicode(false);

           

            modelBuilder.Entity<users>()
                .Property(e => e.avatar)
                .IsUnicode(false);
        }
    }
}
