using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using MovieMaster.Data.Dto;
using NUnit.Framework;

namespace MovieMaster.Data.Tests
{
    public class MovieRepositoryTests
    {
        private readonly List<MovieDto> _movies = new List<MovieDto>
        {
            new MovieDto { Id = "1", Title = "Encanto" , Year = "2013" , Actors = "Laz", ImdbRating = "6.7",LastUpdated = DateTime.Now},
            new MovieDto { Id = "2", Title = "Inception",  Year = "2014" ,Actors = "Fedric",ImdbRating = "7.2", LastUpdated = DateTime.Now},
            new MovieDto { Id = "3", Title = "King Kong",  Year = "2013" ,Actors = "Jazz",ImdbRating = "5.7", LastUpdated = DateTime.Now},
        };
        private MovieRepository _fixture;
        private Mock<ILogger<MovieRepository>> _logger;

        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger<MovieRepository>>();
            _fixture = new MovieRepository(_movies, _logger.Object);

        }

        [Test]
        public async Task GetAllMoviesAsync_ReturnsAllMovies()
        {
            var result = await _fixture.GetAllMoviesAsync();
            Assert.AreEqual(3, result.Count());
        }

        [Test]
        public async Task GetMovieByYearAsync_WhenYearIsInvalid_ReturnNull()
        {
            var result = await _fixture.GetAllMoviesByYearAsync("INVALID_YEAR");
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetMovieByYearAsync_WhenYearIsValid_ReturnMoviesList()
        {
            var result = await _fixture.GetAllMoviesByYearAsync("2013");
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public async Task GetMovieByIdAsync_WhenIdIsInvalid_ReturnNull()
        {
            var result = await _fixture.GetMovieByIdAsync("INVALID_ID");
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetMovieByIdAsync_WhenIdIsValid_ReturnMovie()
        {
            var result = await _fixture.GetMovieByIdAsync("2");
            Assert.AreEqual("Inception", result.Title);
        }

        [Test]
        public async Task CreateMovieAsync_WhenMovieIsValid_ReturnMovie()
        {
            var result = await _fixture.CreateMovieAsync(_movies[0]);
            Assert.AreEqual("Encanto", result.Title);
        }

        [Test]
        public async Task UpdateMovieAsync_WhenIdIsInvalid_ReturnNull()
        {
            var result = await _fixture.UpdateMovieAsync("INVALID_ID", _movies[0]);
            Assert.IsNull(result);
        }

        [Test]
        public async Task UpdateMovieAsync_WhenIdIsInvalid_UpdatesTheMovie()
        {
            var result = await _fixture.UpdateMovieAsync("2", new MovieDto
            {
                Title = "New Title",
                Year = "2022"
            });

            Assert.AreEqual("New Title", result.Title);
            Assert.AreEqual("2022", result.Year);
        }
    }
}