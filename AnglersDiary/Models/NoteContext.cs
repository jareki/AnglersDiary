using SQLite.CodeFirst;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnglersDiary.Models
{
    public class NoteContext : DbContext
    {
        public DbSet<Note> Notes { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Trophy> Trophies { get; set; }
        public DbSet<NoteTackle> NoteTackles { get; set; }
        public DbSet<Tackle> Tackles { get; set; }
        public DbSet<TackleCategory> TackleCategories { get; set; }
        public DbSet<Catch> Catches { get; set; }
        public DbSet<Specy> Species { get; set; }

        public NoteContext() : base("DefaultConnection")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var sqliteConnectionInitializer = new SqliteCreateDatabaseIfNotExists<NoteContext>(modelBuilder);
            Database.SetInitializer(sqliteConnectionInitializer);
        }
    }
}
