using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Security.Cryptography.Xml;
using WebApplication1.Models;

namespace WebApplication1
{
    public class MovieDBContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(@"Server=.\;DataBase=MoviesDB;Trusted_Connection=True;Encrypt=false");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            /////////customers///////////

            modelBuilder.Entity<customer>()
                .HasKey("Id");

            modelBuilder.Entity<customer>()
               .Property("Id")
                .UseIdentityColumn();

            modelBuilder.Entity<customer>()
                .Property("FirstName")
                 .IsRequired()
                 .HasMaxLength(50);

            modelBuilder.Entity<customer>()
                .Property("LastName")
                 .IsRequired()
                 .HasMaxLength(50);

            modelBuilder.Entity<customer>()
             .Property("BirthDate");

            modelBuilder.Entity<customer>()
             .Property("PhoneNumber")
              .IsRequired();

            modelBuilder.Entity<customer>()
             .Property("Address")
              .IsRequired();
        

        //////////Customer_movie///////////
        modelBuilder.Entity<customer_movie>()
            .HasKey(x => new { x.CustomerId, x.MovieId });

            modelBuilder.Entity<customer_movie>()
                 .HasOne(x => x.Customer)
                 .WithMany(x => x.CustomersOfMovies)
                 .HasForeignKey(x => x.CustomerId)
                 .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<customer_movie>()
                 .HasOne(x => x.Movie)
                 .WithMany(x => x.MoviesOfCustomers)
                 .HasForeignKey(x => x.MovieId);

            //////////Movie///////////

            modelBuilder.Entity<Movie>()
              .HasMany(x => x.MoviesOfCustomers)
              .WithOne(x => x.Movie)
              .HasForeignKey(x => x.MovieId);

            //////////////Producer/////////
            modelBuilder.Entity<Producer>()
            .HasMany(x => x.Movies)
           .WithOne(x => x.Producer)
            .HasForeignKey(x => x.ProducerId);

         //////////////////CustomerFeedBack////////////  Updated Code
            modelBuilder.Entity<CustomerFeedBack>()
           .Property("Comments")
           .HasColumnType("nvarchar(max)");
        }

        public DbSet<Movie> Movie { get; set; }
        public DbSet<customer> Customer { get; set; }
        public DbSet<Producer> Producer { get; set; }
        public DbSet<customer_movie> Customer_movie { get; set; }
        public DbSet <CustomerFeedBack> CustomerFeedBack { get; set; }
    }





}
