using System.Collections.Generic;
using System.Threading.Tasks;
using MovieMaster.Service.Model;
using MovieMaster.Service.ViewModel;

namespace MovieMaster.Service.Interfaces
{
    public interface IMovieService
    {
        /// <summary>
        /// Get all movies.
        /// </summary>
        /// <returns>An Enumerable list of Movie objects.</returns>
        Task<IEnumerable<Movie>> FindAllAsync();

        /// <summary>
        /// Gets a movie by Id.
        /// </summary>
        /// <param name="id">Id of the movie.</param>
        /// <returns>A <see cref="Movie"/> object.</returns>
        Task<Movie> FindMovieByIdAsync(string id);

        /// <summary>
        /// Get all movies by year.
        /// </summary>
        /// <param name="year"> Year of the movie.</param>
        /// <returns>An Enumerable list of Movie objects. </returns>
        Task<IEnumerable<Movie>> FindAllMoviesByYearAsync(string year);

        /// <summary>
        /// Create a new movie.
        /// </summary>
        /// <param name="movie"> New movie information to create.</param>
        /// <returns> created new movie.</returns>
        Task<CreateMovieResult> CreateMovieAsync(MovieViewModel movie);

        /// <summary>
        /// Updates the specific movie.
        /// </summary>
        /// <param name="movie">Movie to update.</param>
        /// <returns>The updated movie.</returns>
        Task<UpdateMovieResult> UpdateMovieAsync(string id, MovieViewModel movie);
    }
}