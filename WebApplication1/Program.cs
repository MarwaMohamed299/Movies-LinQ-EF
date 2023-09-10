using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {


            ////////MovieDBContext db = new MovieDBContext();
            ////////db.Producer.Add(new Producer() { ProducerId = 1, Country = "USA", CompanyName = "ABC" });

            ////////using var context = new MovieDBContext();

            using (var context = new MovieDBContext())
            {

                if (context.Database.EnsureCreated())
                {

                    MovieDBContext db = new MovieDBContext();
                    db.Database.Migrate();

                    var producer1 = new Producer { CompanyName = "ABC Studios", Country = "USA" };
                    var producer2 = new Producer { CompanyName = "XYZ Pictures", Country = "Canada" };
                    var producer3 = new Producer { CompanyName = "123 Productions", Country = "France" };

                    context.Producer.AddRange(producer1, producer2, producer3);

                    var movie1 = new Movie { Title = "The Avengers", Duration = 143, Rating = "PG-13", Producer = producer1 };
                    var movie2 = new Movie { Title = "The Dark Knight", Duration = 152, Rating = "PG-13", Producer = producer1 };
                    var movie3 = new Movie { Title = "Inception", Duration = 148, Rating = "PG-13", Producer = producer1 };
                    var movie4 = new Movie { Title = "Jurassic Park", Duration = 127, Rating = "PG-13", Producer = producer2 };
                    var movie5 = new Movie { Title = "Titanic", Duration = 194, Rating = "PG-13", Producer = producer2 };
                    var movie6 = new Movie { Title = "Deadpool", Duration = 108, Rating = "R", Producer = producer2 };
                    var movie7 = new Movie { Title = "Amélie", Duration = 122, Rating = "R", Producer = producer3 };

                    context.Movie.AddRange(movie1, movie2, movie3, movie4, movie5, movie6, movie7);

                    var customer1 = new customer { FirstName = "John", LastName = "Doe", Address = "123 Main St", BirthDate = new DateTime(1985, 1, 1), PhoneNumber = "555-1234" };
                    var customer2 = new customer { FirstName = "Jane", LastName = "Smith", Address = "456 Elm St", BirthDate = new DateTime(1990, 2, 2), PhoneNumber = "555-5678" };
                    var customer3 = new customer { FirstName = "Bob", LastName = "Johnson", Address = "789 Oak St", BirthDate = new DateTime(1980, 3, 3), PhoneNumber = "555-9012" };

                    context.Customer.AddRange(customer1, customer2, customer3);

                    var rental1 = new customer_movie { Customer = customer1, Movie = movie1, TimeRented = DateTime.Now, DueDate = DateTime.Now.AddDays(7) };
                    var rental2 = new customer_movie { Customer = customer1, Movie = movie2, TimeRented = DateTime.Now, DueDate = DateTime.Now.AddDays(7) };
                    var rental3 = new customer_movie { Customer = customer2, Movie = movie3, TimeRented = DateTime.Now, DueDate = DateTime.Now.AddDays(7) };
                    var rental4 = new customer_movie { Customer = customer2, Movie = movie4, TimeRented = DateTime.Now, DueDate = DateTime.Now.AddDays(7) };
                    var rental5 = new customer_movie { Customer = customer2, Movie = movie5, TimeRented = DateTime.Now, DueDate = DateTime.Now.AddDays(7) };
                    var rental6 = new customer_movie { Customer = customer3, Movie = movie6, TimeRented = DateTime.Now, DueDate = DateTime.Now.AddDays(7) };
                    var rental7 = new customer_movie { Customer = customer3, Movie = movie7, TimeRented = DateTime.Now, DueDate = DateTime.Now.AddDays(7) };
                    var rental8 = new customer_movie { Customer = customer3, Movie = movie1, TimeRented = DateTime.Now, DueDate = DateTime.Now.AddDays(7) };
                    var rental9 = new customer_movie { Customer = customer3, Movie = movie2, TimeRented = DateTime.Now, DueDate = DateTime.Now.AddDays(7) };
                    var rental10 = new customer_movie { Customer = customer3, Movie = movie3, TimeRented = DateTime.Now, DueDate = DateTime.Now.AddDays(7) };

                    context.Customer_movie.AddRange(rental1, rental2, rental3, rental4, rental5, rental6, rental7, rental8, rental9, rental10);

                    context.SaveChanges();


                }
                //////////////query 1 [Top 3 rented movies//////////
                ///
                var topRentedMovies = context.Customer_movie
                      .OrderByDescending(m => m.TimeRented)
                      .Take(3)
                      .Select(m => m.Movie)
                      .ToList();

                Console.WriteLine("Top 3 rented movies:");
                foreach (var movie in topRentedMovies)
                {
                    Console.WriteLine(movie.Title);

                }

                Console.WriteLine("===============================");
                //////////////query 2 [Producer with most movies and movie count//////////

              
                    var producerWithMostMovies = context.Producer
                    .Include(m=>m.Movies)
                        .OrderByDescending(p => p.Movies.SelectMany(m => m.MoviesOfCustomers).Count(mc => mc.Movie != null))
                        .FirstOrDefault();

                    if (producerWithMostMovies != null)
                    {
                        Console.WriteLine($"Producer with most movies: {producerWithMostMovies.CompanyName}");
                        Console.WriteLine($"Movie count: {producerWithMostMovies.Movies.Count()}");
                    }
                    else
                    {
                        Console.WriteLine("No producers found.");
                    }

                Console.WriteLine("===============================");

                ///////////////////query3 [Customer  ordered by their rental accounts]/////
                var customersByRentalCount = context.Customer
                      .Include(c => c.CustomersOfMovies)
                      .OrderByDescending(c => c.CustomersOfMovies.Count())
                      .ToList();

                foreach (var customer in customersByRentalCount)
                {
                    Console.WriteLine($"{customer.FirstName} {customer.LastName} - Rental Accounts: {customer.CustomersOfMovies.Count()}");
                }


                Console.WriteLine("===============================");
                ///////////////////query 4 [Full information rentals ]/////

                var rentalInformation = context.Customer_movie
                       .Include(m => m.Customer)
                       .Include(m => m.Movie)
                           .ThenInclude(m => m.Producer)
                       .Select(m => new {
                           CustomerName = m.Customer.FirstName + " " + m.Customer.LastName,
                           Address = m.Customer.Address,
                           PhoneNumber = m.Customer.PhoneNumber,
                           MovieTitle = m.Movie.Title,
                           ProducerName = m.Movie.Producer.CompanyName,
                           ProducerCountry = m.Movie.Producer.Country,
                           RentDate = m.TimeRented,
                           OverDueDate = m.DueDate
                       })
                       .ToList();

                foreach (var info in rentalInformation)
                {
                    Console.WriteLine($"Customer Name: {info.CustomerName}\nAddress: {info.Address}\nPhone Number: {info.PhoneNumber}\nMovie Title: {info.MovieTitle}\nProducer Name: {info.ProducerName}\nProducer Country: {info.ProducerCountry}\nRent Date: {info.RentDate}\nOverdue Date: {info.OverDueDate}\n");
                }


                Console.WriteLine("===============================");


                ///////////////////query 5 [overdue rentals ]/////


                var overdueRentals = context.Customer_movie
                    .Include(cm => cm.Customer)
                    .Include(cm => cm.Movie)
                    .Where(cm => cm.DueDate < DateTime.Now)
                    .OrderBy(cm => cm.DueDate)
                    .Select(cm => new {
                        CustomerName = cm.Customer.FirstName + " " + cm.Customer.LastName,
                        MovieTitle = cm.Movie.Title,
                        RentalDate = cm.TimeRented
                    })
                    .ToList();

                foreach (var rental in overdueRentals)
                {
                    Console.WriteLine($"Customer Name: {rental.CustomerName}\nMovie Title: {rental.MovieTitle}\nRental Date: {rental.RentalDate}\n");
                }


            }
        }
    }
}
