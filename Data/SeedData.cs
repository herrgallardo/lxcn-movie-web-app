using Microsoft.EntityFrameworkCore;
using lxcn_movie_web_app.Models;

namespace lxcn_movie_web_app.Data
{
    /// <summary>
    /// Provides sample data to seed the database with initial movie records
    /// Contains popular movies with real TMDB poster images
    /// </summary>
    public static class SeedData
    {
        /// <summary>
        /// Initialize the database with sample movie data if no movies exist
        /// </summary>
        /// <param name="serviceProvider">Service provider for dependency injection</param>
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                // Check if any movies already exist
                if (context.Movies.Any())
                {
                    return; // Database has been seeded
                }

                // Add sample movies with real TMDB poster images
                context.Movies.AddRange(
                    new Movie
                    {
                        Title = "Joker",
                        ReleaseDate = DateTime.Parse("2019-10-4"),
                        Genre = "Drama",
                        Price = 8.99M,
                        Description = "Arthur Fleck, a party clown, leads an impoverished life with his ailing mother. However, when society shuns him and brands him as a freak, he decides to embrace the life of crime and chaos in Gotham City.",
                        ImageUrl = "https://image.tmdb.org/t/p/w500/udDclJoHjfjb8Ekgsd4FDteOkCU.jpg",
                        Rating = "R",
                        Duration = 122,
                        DateCreated = DateTime.Now.AddDays(-10)
                    },
                    new Movie
                    {
                        Title = "Gladiator",
                        ReleaseDate = DateTime.Parse("2000-5-5"),
                        Genre = "Action",
                        Price = 9.99M,
                        Description = "Commodus takes over power and demotes Maximus, one of the preferred generals of his father and a loved man. As a result, Maximus is relegated to fighting till death as a gladiator.",
                        ImageUrl = "https://image.tmdb.org/t/p/w500/ty8TGRuvJLPUmAR1H1nRIsgwvim.jpg",
                        Rating = "R",
                        Duration = 155,
                        DateCreated = DateTime.Now.AddDays(-9)
                    },
                    new Movie
                    {
                        Title = "Avengers: Endgame",
                        ReleaseDate = DateTime.Parse("2019-4-26"),
                        Genre = "Action",
                        Price = 12.99M,
                        Description = "Avengers: Endgame is a 2019 American superhero film based on the Marvel Comics superhero team the Avengers. The grave course of events set in motion by Thanos that wiped out half the universe and fractured the Avengers ranks compels the remaining Avengers to take one final stand.",
                        ImageUrl = "https://image.tmdb.org/t/p/w500/or06FN3Dka5tukK1e9sl16pB3iy.jpg",
                        Rating = "PG-13",
                        Duration = 181,
                        DateCreated = DateTime.Now.AddDays(-8)
                    },
                    new Movie
                    {
                        Title = "The Dark Knight",
                        ReleaseDate = DateTime.Parse("2008-7-18"),
                        Genre = "Action",
                        Price = 8.99M,
                        Description = "Batman begins an assault on organised crime with his new ally, District Attorney Harvey Dent. They are successful, but they soon find themselves prey to a rising criminal mastermind known as the Joker.",
                        ImageUrl = "https://image.tmdb.org/t/p/w500/qJ2tW6WMUDux911r6m7haRef0WH.jpg",
                        Rating = "PG-13",
                        Duration = 152,
                        DateCreated = DateTime.Now.AddDays(-7)
                    },
                    new Movie
                    {
                        Title = "Shutter Island",
                        ReleaseDate = DateTime.Parse("2010-2-19"),
                        Genre = "Thriller",
                        Price = 7.99M,
                        Description = "Teddy Daniels and Chuck Aule, two US marshals, are sent to an asylum on a remote island in order to investigate the disappearance of a patient, where Teddy uncovers a shocking truth about the place.",
                        ImageUrl = "https://image.tmdb.org/t/p/w500/kve20tXwUZpu4GUX8l6X7Z4jmL6.jpg",
                        Rating = "R",
                        Duration = 138,
                        DateCreated = DateTime.Now.AddDays(-6)
                    },
                    new Movie
                    {
                        Title = "Inception",
                        ReleaseDate = DateTime.Parse("2010-7-16"),
                        Genre = "Sci-Fi",
                        Price = 9.99M,
                        Description = "A thief who steals corporate secrets through the use of dream-sharing technology is given the inverse task of planting an idea into the mind of a C.E.O., but his tragic past may doom the project and his team to disaster.",
                        ImageUrl = "https://image.tmdb.org/t/p/w500/9gk7adHYeDvHkCSEqAvQNLV5Uge.jpg",
                        Rating = "PG-13",
                        Duration = 148,
                        DateCreated = DateTime.Now.AddDays(-5)
                    },
                    new Movie
                    {
                        Title = "Pulp Fiction",
                        ReleaseDate = DateTime.Parse("1994-10-14"),
                        Genre = "Crime",
                        Price = 6.99M,
                        Description = "The lives of two mob hitmen, a boxer, a gangster and his wife intertwine in four tales of violence and redemption.",
                        ImageUrl = "https://image.tmdb.org/t/p/w500/d5iIlFn5s0ImszYzBPb8JPIfbXD.jpg",
                        Rating = "R",
                        Duration = 154,
                        DateCreated = DateTime.Now.AddDays(-4)
                    },
                    new Movie
                    {
                        Title = "The Matrix",
                        ReleaseDate = DateTime.Parse("1999-3-31"),
                        Genre = "Sci-Fi",
                        Price = 8.99M,
                        Description = "When a beautiful stranger leads computer hacker Neo to a forbidding underworld, he discovers the shocking truth--the life he knows is the elaborate deception of an evil cyber-intelligence.",
                        ImageUrl = "https://image.tmdb.org/t/p/w500/f89U3ADr1oiB1s9GkdPOEpXUk5H.jpg",
                        Rating = "R",
                        Duration = 136,
                        DateCreated = DateTime.Now.AddDays(-3)
                    },
                    new Movie
                    {
                        Title = "Forrest Gump",
                        ReleaseDate = DateTime.Parse("1994-7-6"),
                        Genre = "Drama",
                        Price = 7.99M,
                        Description = "The presidencies of Kennedy and Johnson, the Vietnam War, the Watergate scandal and other historical events unfold from the perspective of an Alabama man with an IQ of 75, whose only desire is to be reunited with his childhood sweetheart.",
                        ImageUrl = "https://image.tmdb.org/t/p/w500/arw2vcBveWOVZr6pxd9XTd1TdQa.jpg",
                        Rating = "PG-13",
                        Duration = 142,
                        DateCreated = DateTime.Now.AddDays(-2)
                    },
                    new Movie
                    {
                        Title = "The Shawshank Redemption",
                        ReleaseDate = DateTime.Parse("1994-9-23"),
                        Genre = "Drama",
                        Price = 9.99M,
                        Description = "Two imprisoned men bond over a number of years, finding solace and eventual redemption through acts of common decency.",
                        ImageUrl = "https://image.tmdb.org/t/p/w500/q6y0Go1tsGEsmtFryDOJo3dEmqu.jpg",
                        Rating = "R",
                        Duration = 142,
                        DateCreated = DateTime.Now.AddDays(-1)
                    }
                );

                context.SaveChanges();
            }
        }
    }
}