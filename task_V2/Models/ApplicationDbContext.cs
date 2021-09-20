using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace task_V2.Models
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
         : base(options)
        {
        }
        public DbSet<Kandidati> Kandidati { get; set; }
        public DbSet<Gradovi> Gradovi { get; set; }
        public DbSet<Rezultati> Rezultati { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Kandidati>().HasData(
                new Kandidati
                {
                    ID = 1,
                    Kratica = "DT",
                    ImeKandidata = "Donald Trump"
                });

            modelBuilder.Entity<Kandidati>().HasData(
                 new Kandidati
                 {
                     ID = 2,
                     Kratica = "HC",
                     ImeKandidata = "Hillary Clinton"
                 });

            modelBuilder.Entity<Kandidati>().HasData(
                 new Kandidati
                 {
                     ID = 3,
                     Kratica = "JB",
                     ImeKandidata = " Joe Biden"
                 });

            modelBuilder.Entity<Kandidati>().HasData(
               new Kandidati
               {
                   ID = 4,
                   Kratica = "JFK",
                   ImeKandidata = "John F. Kennedy"
               });

            modelBuilder.Entity<Kandidati>().HasData(
               new Kandidati
               {
                   ID = 5,
                   Kratica = "JR",
                   ImeKandidata = "Jack Randall"
               });
        }
    }
}
