using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using WebApplication1.GraphTypes;

namespace WebApplication1.DataContexts
{
    public class BookDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }

        public BookDbContext(DbContextOptions<BookDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().HasData(
                new Book { Id = 1, Name = "Book1" },
                new Book { Id = 2, Name = "Book2" },
                new Book { Id = 3, Name = "Book3" },
                new Book { Id = 4, Name = "Book4" });

            base.OnModelCreating(modelBuilder);
        }
    }
}
