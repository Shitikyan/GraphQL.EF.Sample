using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication1.GraphTypes;

namespace WebApplication1.DataContexts
{
    public class AuthorDbContext : DbContext
    {
        public DbSet<Author> Author { get; set; }

        public AuthorDbContext(DbContextOptions<AuthorDbContext> dbContextOptions) : base(dbContextOptions)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Author>().HasData(
                new Author { Id = 1, Name = "Remarque" },
                new Author { Id = 2, Name = "Hemingway" },
                new Author { Id = 3, Name = "Scott Fitzgerald" });

            base.OnModelCreating(modelBuilder);
        }
    }
}
