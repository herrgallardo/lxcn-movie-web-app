using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lxcn_movie_web_app.Models
{
    /// <summary>
    /// Movie entity representing a movie in the database
    /// Contains all the essential information about a movie including title, genre, release date, and price
    /// </summary>
    public class Movie
    {
        /// <summary>
        /// Primary key for the movie entity
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Movie title - required field with maximum length validation
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters")]
        [Display(Name = "Movie Title")]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Release date of the movie
        /// </summary>
        [Required]
        [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ReleaseDate { get; set; }

        /// <summary>
        /// Movie genre (Comedy, Action, Drama, etc.)
        /// </summary>
        [Required]
        [StringLength(50, ErrorMessage = "Genre cannot be longer than 50 characters")]
        public string Genre { get; set; } = string.Empty;

        /// <summary>
        /// Movie price for rental or purchase
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, 999.99, ErrorMessage = "Price must be between $0.01 and $999.99")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Price { get; set; }

        /// <summary>
        /// Brief description of the movie plot
        /// </summary>
        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters")]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// URL or path to the movie poster image
        /// </summary>
        [Display(Name = "Poster Image")]
        public string? ImageUrl { get; set; }

        /// <summary>
        /// Movie rating (e.g., PG, PG-13, R, etc.)
        /// </summary>
        [StringLength(10, ErrorMessage = "Rating cannot be longer than 10 characters")]
        public string? Rating { get; set; }

        /// <summary>
        /// Movie duration in minutes
        /// </summary>
        [Display(Name = "Duration (minutes)")]
        [Range(1, 600, ErrorMessage = "Duration must be between 1 and 600 minutes")]
        public int? Duration { get; set; }

        /// <summary>
        /// When the movie record was created in the database
        /// </summary>
        [Display(Name = "Date Added")]
        public DateTime DateCreated { get; set; } = DateTime.Now;

        /// <summary>
        /// User ID of who added this movie (for tracking purposes)
        /// </summary>
        public string? CreatedBy { get; set; }
    }
}