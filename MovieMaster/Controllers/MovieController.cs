using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MovieMaster.Service;
using MovieMaster.Service.Interfaces;
using MovieMaster.Service.Model;
using MovieMaster.Service.ViewModel;
using Swashbuckle.AspNetCore.Annotations;

namespace MovieMaster.Controllers
{
    /// <summary>
    /// Handles CRUD operations on movies.
    /// </summary>
    [ApiController]
    [Route("movie")]
    [Produces("application/json")]
    public class MovieController : ControllerBase
    {
        private readonly ILogger<MovieController> _logger;
        private readonly IMovieService _movieService;

        /// <summary>
        /// Creates a new instance of <see cref="MovieController"/>.
        /// </summary>
        /// <param name="logger">An instance of <see cref="ILogger"/>.</param>
        /// <param name="movieService">An instance of <see cref="IMovieService"/>.</param>
        public MovieController(ILogger<MovieController> logger, IMovieService movieService)
        {
            _logger = logger;
            _movieService = movieService;
        }

        /// <summary>
        /// Retrieves a list of movies
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK, "Movies", typeof(Movie[]))]
        public async Task<IActionResult> GetAsync()
        {
            var movies = await _movieService.FindAllAsync();
            _logger.LogInformation("GET Movies endpoint called.");
            return Ok(movies);
        }

        /// <summary>
        /// Retrieves movies by Year.
        /// </summary>
        /// <param name="year"> Year of the movies to get.</param>
        /// <returns>Movies by year.</returns>
        [HttpGet]
        [Route("{year}/movies")]
        [SwaggerResponse(StatusCodes.Status200OK, "Movies", typeof(Movie[]))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Movies does not exist on this year")]
        public async Task<IActionResult> GetByYearAsync(string year)
        {
            var movies = await _movieService.FindAllMoviesByYearAsync(year);
            _logger.LogInformation("GET MoviesByYear endpoint called.");
            return movies == null
                ? (IActionResult)NotFound()
                : Ok(movies);
        }


        /// <summary>
        /// Retrieves a specific movie by ID.
        /// </summary>
        /// <param name="id">ID of the movie to get.</param>
        /// <returns>A Movie.</returns>
        [HttpGet]
        [Route("{id}")]
        [SwaggerResponse(StatusCodes.Status200OK, "Movie", typeof(Movie))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Movie does not exist")]
        public async Task<IActionResult> GetByIdAsync(string id)
        {
            var movie = await _movieService.FindMovieByIdAsync(id);
            _logger.LogInformation("GET MoviesById endpoint called.");
            return movie == null
                ? (IActionResult)NotFound()
                : Ok(movie);
        }

        /// <summary>
        /// Create a movie.
        /// </summary>
        /// <param name="movie"> Movie details.</param>
        /// <returns>The created new movie.</returns>
        [HttpPost]
        [SwaggerResponse(StatusCodes.Status201Created, "Movie", typeof(MovieViewModel))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Unknown Error")]
        public async Task<IActionResult> CreateMovieAsync([FromBody] MovieViewModel movie)
        {
            var result = await _movieService.CreateMovieAsync(movie);
            _logger.LogInformation("Create Movie endpoint called.");
            switch (result.Status)
            {
                case CreateMovieStatus.Created: return Created("", result.Movie);
                default: return StatusCode(500);
            }
        }

        /// <summary>
        /// Update a movie.
        /// </summary>
        /// <param name="id">ID of the movie.</param>
        /// <param name="movie">Details of movie.</param>
        /// <returns>The updated Movie.</returns>
        [HttpPut]
        [Route("{id}")]
        [SwaggerResponse(StatusCodes.Status200OK, "Movie", typeof(MovieViewModel))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Movie does not exist")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Unknown Error")]
        public async Task<IActionResult> UpdateMovieAsync(string id, [FromBody] MovieViewModel movie)
        {
            var result = await _movieService.UpdateMovieAsync(id, movie);
            _logger.LogInformation("Update Movie endpoint called.");
            switch (result.Status)
            {
                case UpdateMovieStatus.Updated: return Ok(result.Movie);
                case UpdateMovieStatus.NotFound: return NotFound();
                default: return StatusCode(500);
            }
        }
    }
}