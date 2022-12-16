using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using MovieMaster.Data;
using MovieMaster.Data.Dto;
using MovieMaster.Data.Interfaces;
using MovieMaster.Service.Interfaces;
using MovieMaster.Service.Model;
using MovieMaster.Service.ViewModel;

namespace MovieMaster.Service
{
    /// <inheritdoc />
    public sealed class MovieService : IMovieService
    {
        private readonly IMapper _mapper;
        private readonly IMovieRepository _movieRepository;
        private readonly ILogger<MovieService> _logger;

        public MovieService(IMapper mapper, IMovieRepository movieRepository, ILogger<MovieService> logger)
        {
            _mapper = mapper;
            _movieRepository = movieRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<Movie>> FindAllAsync()
        {
            var movies = await _movieRepository.GetAllMoviesAsync();
            _logger.LogInformation("GET movies service called.");
            return movies.Select(m => _mapper.Map<Movie>(m));
        }

        public async Task<IEnumerable<Movie>> FindAllMoviesByYearAsync(string year)
        {
            var movies = await _movieRepository.GetAllMoviesByYearAsync(year);
            _logger.LogInformation("Find all movies by year service called.");
            return !movies.Any()
                ? null
                : movies.Select(m => _mapper.Map<Movie>(m));
        }

        public async Task<Movie> FindMovieByIdAsync(string id)
        {
            var movie = await _movieRepository.GetMovieByIdAsync(id);
            _logger.LogInformation("Find all movies by id service called.");
            return movie == null
                ? null
                : _mapper.Map<Movie>(movie);
        }


        public async Task<CreateMovieResult> CreateMovieAsync(MovieViewModel movie)
        {

            var movieDto = _mapper.Map<MovieDto>(movie);

            // Create the movie
            var result = await _movieRepository.CreateMovieAsync(movieDto);
            _logger.LogInformation("Create movie service retrieved existing movie.");

            // Return Error
            if (result == null)
            {
                _logger.LogCritical("Create movie service error, results returned null.");
                return new CreateMovieResult { Status = CreateMovieStatus.Error };
            }

            _logger.LogInformation("Create movie service success.");
            // Return Success
            return new CreateMovieResult
            {
                Status = CreateMovieStatus.Created,
                Movie = _mapper.Map<Movie>(result)
            };
        }

        public async Task<UpdateMovieResult> UpdateMovieAsync(string id, MovieViewModel movie)
        {
            // Check the movie exists
            var movieInDb = await _movieRepository.GetMovieByIdAsync(id);
            _logger.LogInformation("Update movie service retrieved existing movie.");

            if (movieInDb == null)
            {
                _logger.LogWarning("Update movie service called, movie does not exist on db for the given id.");
                return new UpdateMovieResult { Status = UpdateMovieStatus.NotFound };
            }

            var movieDto = _mapper.Map<MovieDto>(movie);

            // Update the movie
            var result = await _movieRepository.UpdateMovieAsync(id, movieDto);
            _logger.LogInformation("Update movie service called.");

            if (result == null)
            {
                _logger.LogCritical("Update movie service error, result returned null");
                return new UpdateMovieResult { Status = UpdateMovieStatus.Error };
            }

            _logger.LogInformation("Update movie service success.");
            // Return success
            return new UpdateMovieResult
            {
                Status = UpdateMovieStatus.Updated,
                Movie = _mapper.Map<Movie>(result)
            };
        }
    }
}