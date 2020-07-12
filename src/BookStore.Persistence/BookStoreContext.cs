using BookStore.Persistence.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace BookStore.Persistence
{
    public class BookStoreContext : IdentityDbContext<ApplicationUser>
    {
        public BookStoreContext()
        {

        }
        public BookStoreContext(DbContextOptions<BookStoreContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=Database.db");
            }
        }

        public DbSet<Book> Books { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Author> Authors { get; set; }

        public DbSet<Publisher> Publishers { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderLine> OrderLines { get; set; }

        public DbSet<FavoriteBook> FavoriteBooks { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Book>()
                .HasOne(s => s.Author)
                .WithMany();

            builder.Entity<Book>()
               .HasOne(s => s.Category)
               .WithMany();

            builder.Entity<Book>()
              .HasOne(s => s.Publisher)
              .WithMany();

            builder.Entity<Order>()
              .HasMany(s => s.OrderLines);


            builder.Entity<Author>().HasData(BookStoreSeeder.GetAuthors());

            builder.Entity<Category>()
                .HasData(BookStoreSeeder.GetCategories());

            builder.Entity<Publisher>()
                .HasData(BookStoreSeeder.GetPublishers());

            builder.Entity<Book>()
                .HasData(BookStoreSeeder.GetBooks());



        }
    }
}
