using Microsoft.EntityFrameworkCore;
using Grigorova_Server.Models;

namespace Grigorova_Server.Data
{
    public class Grigorova_ServerContext : DbContext
    {
        public Grigorova_ServerContext(DbContextOptions<Grigorova_ServerContext> options)
            : base(options)
        {
        }

        // --- Таблицы ---
        public DbSet<Author> Authors { get; set; } = null!;
        public DbSet<Book> Books { get; set; } = null!;
        public DbSet<Genres> Genres { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Borrowings> Borrowings { get; set; } = null!;
        public DbSet<BookAuthor> BookAuthors { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- Настройка Author ---
            modelBuilder.Entity<Author>()
                .HasKey(a => a.AuthorId);

            // --- Настройка Genre ---
            modelBuilder.Entity<Genres>()
                .HasKey(g => g.GenresId);

            // --- Настройка User ---
            modelBuilder.Entity<User>()
                .HasKey(u => u.UserId);

            // --- Настройка Book ---
            modelBuilder.Entity<Book>()
                .HasKey(b => b.BookId);

            // --- Настройка Borrowings ---
            modelBuilder.Entity<Borrowings>()
                .HasKey(b => b.BorrowId);

            modelBuilder.Entity<Borrowings>()
                .Property(b => b.UserId)
                .IsRequired();

            modelBuilder.Entity<Borrowings>()
                .Property(b => b.BookId)
                .IsRequired();

            // --- Настройка связи "многие ко многим" между Book и Author ---
            modelBuilder.Entity<BookAuthor>()
                .HasKey(ba => ba.BookAuthorId);

            modelBuilder.Entity<BookAuthor>()
                .HasOne(ba => ba.Book)
                .WithMany(b => b.BookAuthors)
                .HasForeignKey(ba => ba.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BookAuthor>()
                .HasOne(ba => ba.Author)
                .WithMany(a => a.BookAuthors)
                .HasForeignKey(ba => ba.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
