using Core.Models.TalentsFinderModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.TalentsFinder.Services.SQLiteProvider
{
    public class SQLiteDbContext : DbContext
    {
        private string _connection;

        public SQLiteDbContext(ISQLiteDatabase connection):base()
        {
            _connection = connection.GetConnection();

            // Create database if not there. This will also ensure the data seeding will happen.
            try
            {
                Database.EnsureCreated();
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }

        public virtual DbSet<CriteriaModel> Criteria { get; set; }
        public virtual DbSet<TecNamesModel> CriteriaTechhNames { get; set; }

        public virtual DbSet<TalentModel> Talents { get; set; }
        public virtual DbSet<Name> TalentsName { get; set; }
        public virtual DbSet<Technology> TalentsTech{ get; set; }

        public virtual DbSet<StatusModel> Status { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={_connection}");
            
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // EF Core does not support creating a composite key using Key attribute
            //modelBuilder.Entity<PicturesModel>().HasKey(p => new { p.IdPicture, p.IdProduct });

            //// Add a converter to URI
            //modelBuilder.Entity<TecModel>()
            //    .Property(p => p.Url)
            //    .HasConversion(v => v.ToString(), v => new Uri(v));

            //// Add a converter to URI
            //modelBuilder.Entity<TecModel>()
            //    .Property(p => p.Logo)
            //    .HasConversion(v => v.ToString(), v => new Uri(v));

            // One to One
            modelBuilder.Entity<Name>()
            .HasOne(p => p.TalentModel)
            .WithOne(b => b.Name)
            .HasForeignKey<Name>(c => c.TalentModelId)
            .OnDelete(DeleteBehavior.Cascade);

            // One to Many
            modelBuilder.Entity<Technology>()
            .HasOne(p => p.Talents)
            .WithMany(b => b.Technologies)
            .OnDelete(DeleteBehavior.Cascade);

            // One to Many
            modelBuilder.Entity<TecNamesModel>()
            .HasOne(p => p.CriteriaModel)
            .WithMany(b => b.TecNamesModel)
            .OnDelete(DeleteBehavior.Cascade);

            // Add a converter to URI
            modelBuilder.Entity<TalentModel>()
                .Property(p => p.Picture)
                .HasConversion(v => v.ToString(), v => new Uri(v));

        }
    }
}
