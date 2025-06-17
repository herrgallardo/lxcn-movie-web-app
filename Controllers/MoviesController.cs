using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using lxcn_movie_web_app.Data;
using lxcn_movie_web_app.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace lxcn_movie_web_app.Controllers
{
    /// <summary>
    /// Controller for handling movie CRUD operations with role-based authorization
    /// Implements three-tier access: Admin (full), User (read/update), Non-user (view only)
    /// </summary>
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<MoviesController> _logger;

        public MoviesController(ApplicationDbContext context, ILogger<MoviesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Display list of all movies with search and filter functionality
        /// GET: Movies - Available to everyone (including non-users)
        /// </summary>
        public async Task<IActionResult> Index(string searchString, string movieGenre, string sortOrder)
        {
            // Create query for movies
            var movies = from m in _context.Movies
                         select m;

            // Get list of genres for dropdown filter
            var genreQuery = from m in _context.Movies
                             orderby m.Genre
                             select m.Genre;
            ViewBag.MovieGenres = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(await genreQuery.Distinct().ToListAsync());

            // Apply search filter
            if (!string.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(s => s.Title.Contains(searchString) || s.Description.Contains(searchString));
                ViewBag.CurrentFilter = searchString;
            }

            // Apply genre filter
            if (!string.IsNullOrEmpty(movieGenre))
            {
                movies = movies.Where(x => x.Genre == movieGenre);
                ViewBag.CurrentGenre = movieGenre;
            }

            // Apply sorting
            ViewBag.TitleSortParm = string.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "price_desc" : "Price";
            ViewBag.GenreSortParm = sortOrder == "Genre" ? "genre_desc" : "Genre";

            switch (sortOrder)
            {
                case "title_desc":
                    movies = movies.OrderByDescending(s => s.Title);
                    break;
                case "Date":
                    movies = movies.OrderBy(s => s.ReleaseDate);
                    break;
                case "date_desc":
                    movies = movies.OrderByDescending(s => s.ReleaseDate);
                    break;
                case "Price":
                    movies = movies.OrderBy(s => s.Price);
                    break;
                case "price_desc":
                    movies = movies.OrderByDescending(s => s.Price);
                    break;
                case "Genre":
                    movies = movies.OrderBy(s => s.Genre);
                    break;
                case "genre_desc":
                    movies = movies.OrderByDescending(s => s.Genre);
                    break;
                default:
                    movies = movies.OrderBy(s => s.Title);
                    break;
            }

            return View(await movies.ToListAsync());
        }

        /// <summary>
        /// Display details of a specific movie
        /// GET: Movies/Details/5 - Available to everyone (including non-users)
        /// </summary>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        /// <summary>
        /// Display form to create a new movie
        /// GET: Movies/Create - ADMIN ONLY
        /// </summary>
        [Authorize(Roles = RoleConstants.Admin)]
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Process the creation of a new movie
        /// POST: Movies/Create - ADMIN ONLY
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<IActionResult> Create([Bind("Title,ReleaseDate,Genre,Price,Description,ImageUrl,Rating,Duration")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                // Set audit fields
                movie.DateCreated = DateTime.Now;
                movie.CreatedBy = User.FindFirstValue(ClaimTypes.NameIdentifier);

                _context.Add(movie);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Movie created successfully!";
                return RedirectToAction(nameof(Index));
            }

            return View(movie);
        }

        /// <summary>
        /// Display form to edit an existing movie
        /// GET: Movies/Edit/5 - ADMIN AND USER
        /// </summary>
        [Authorize(Roles = RoleConstants.AdminAndUser)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        /// <summary>
        /// Process the update of an existing movie
        /// POST: Movies/Edit/5 - ADMIN AND USER
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleConstants.AdminAndUser)]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ReleaseDate,Genre,Price,Description,ImageUrl,Rating,Duration,DateCreated,CreatedBy")] Movie movie)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Movie updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        /// <summary>
        /// Display confirmation page for deleting a movie
        /// GET: Movies/Delete/5 - ADMIN ONLY
        /// </summary>
        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        /// <summary>
        /// Process the deletion of a movie
        /// POST: Movies/Delete/5 - ADMIN ONLY
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Movie deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Check if a movie exists in the database
        /// </summary>
        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }

        /// <summary>
        /// AJAX endpoint for searching movies
        /// GET: Movies/Search - Available to everyone
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Search(string term)
        {
            if (string.IsNullOrEmpty(term))
            {
                return Json(new string[0]);
            }

            var movies = await _context.Movies
                .Where(m => m.Title.Contains(term))
                .Select(m => m.Title)
                .Take(10)
                .ToListAsync();

            return Json(movies);
        }

        /// <summary>
        /// Development helper to reset sample data - REMOVE IN PRODUCTION
        /// GET: Movies/ResetData
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ResetData()
        {
            // Only allow in development environment
            var environment = HttpContext.RequestServices.GetService<IWebHostEnvironment>();
            if (environment?.IsDevelopment() != true)
            {
                return NotFound();
            }

            // Clear existing movies
            var existingMovies = await _context.Movies.ToListAsync();
            _context.Movies.RemoveRange(existingMovies);
            await _context.SaveChangesAsync();

            // Reseed with new data
            using (var scope = HttpContext.RequestServices.CreateScope())
            {
                var services = scope.ServiceProvider;
                Data.SeedData.Initialize(services);
            }

            TempData["SuccessMessage"] = "Movie data has been reset with new TMDB posters!";
            return RedirectToAction(nameof(Index));
        }
    }
}