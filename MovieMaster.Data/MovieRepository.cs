using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using MovieMaster.Data.Dto;
using MovieMaster.Data.Interfaces;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

[assembly: InternalsVisibleTo("MovieMaster.Data.Tests")]

namespace MovieMaster.Data
{
    /// <inheritdoc />
    public sealed class MovieRepository : IMovieRepository
    {
        private readonly Dictionary<string, MovieDto> _movies;
        private readonly ILogger<MovieRepository> _logger;

        public MovieRepository(ILogger<MovieRepository> logger)
        {
            _logger = logger;

            var file = File.ReadAllText("Movies.json");
            var movieList = JsonConvert.DeserializeObject<List<MovieDto>>(file);
            _movies = movieList.ToDictionary(m => m.Id);
        }

        internal MovieRepository(IEnumerable<MovieDto> movies, ILogger<MovieRepository> logger)
        {
            _logger = logger;
            _movies = movies.ToDictionary(m => m.Id);
        }


        public Task<IEnumerable<MovieDto>> GetAllMoviesAsync()
        {
            _logger.LogInformation("GET movies respository called.");
            return Task.FromResult(_movies.Values.Skip(0));
        }

        public Task<IEnumerable<MovieDto>> GetAllMoviesByYearAsync(string year)
        {
            var movies = _movies.Values.Where(x => x.Year.Contains(year));
            _logger.LogInformation("GET all movies by year respository called.");
            return movies.Any()
                ? Task.FromResult(movies)
                : Task.FromResult<IEnumerable<MovieDto>>(null);
        }

        public Task<MovieDto> GetMovieByIdAsync(string id)
        {
            _logger.LogInformation("GET movies by id respository called.");
            return _movies.TryGetValue(id, out var movie)
                ? Task.FromResult(movie)
                : Task.FromResult<MovieDto>(null);
        }

        public Task<MovieDto> CreateMovieAsync(MovieDto movie)
        {
            var id = Guid.NewGuid().ToString();
            movie.Id = id;
            movie.LastUpdated = DateTime.UtcNow;

            if (_movies.TryAdd(id, movie))
            {
                _logger.LogInformation("Create movie respository called.");
                return Task.FromResult(movie);
            }
            _logger.LogWarning("Create movie respository return null.");
            return Task.FromResult<MovieDto>(null);
        }

        public Task<MovieDto> UpdateMovieAsync(string id, MovieDto details)
        {
            if (_movies.TryGetValue(id, out var movie))
            {
                movie.Title = details.Title;
                movie.Year = details.Year;
                movie.Actors = details.Actors;
                movie.LastUpdated = DateTime.Now;

                _logger.LogInformation("Update movie respository called.");
                return Task.FromResult(movie);
            }

            _logger.LogWarning("Update movie respository return null.");
            return Task.FromResult<MovieDto>(null);
        }
    }
}