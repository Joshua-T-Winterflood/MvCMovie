using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MvcMovie.Domain;

namespace MvcMovie.Data
{
 public class MvcMovieContext : DbContext
    {
        public MvcMovieContext(DbContextOptions<MvcMovieContext> options)
            : base(options)
        {
        }

        public DbSet<Movie> Movie { get; set; } = default!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the Id property to be an identity column
            modelBuilder.Entity<Movie>()
                .Property(m => m.Id)
                .HasIdentityOptions(startValue: 100); // This sets the Id property to use SQL identity/auto-increment
        }

        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries<Movie>())
            {
                if (entry.Entity.ReleaseDate.Kind == DateTimeKind.Unspecified)
                {
                    entry.Entity.ReleaseDate = DateTime.SpecifyKind(entry.Entity.ReleaseDate, DateTimeKind.Utc);
                }
            }

            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<Movie>())
            {
                if (entry.Entity.ReleaseDate.Kind == DateTimeKind.Unspecified)
                {
                    entry.Entity.ReleaseDate = DateTime.SpecifyKind(entry.Entity.ReleaseDate, DateTimeKind.Utc);
                }
            }

            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}
