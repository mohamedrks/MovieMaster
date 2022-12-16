using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using MovieMaster.Data;
using MovieMaster.Data.Dto;
using MovieMaster.Data.Interfaces;
using MovieMaster.Service.Model;
using MovieMaster.Service.ViewModel;
using NUnit.Framework;

namespace MovieMaster.Service.Tests
{
    public class MovieServiceTests
    {
        private Mock<IMovieRepository> _movieRepository;
        private Mock<ILogger<MovieService>> _logger;
        private MovieService _fixture;

        [SetUp]
        public void Setup()
        {
            var movies = new List<MovieDto>
            {
                new MovieDto { Id = "1", Title = "Encanto" ,Year = "2004"},
                new MovieDto { Id = "2", Title = "Inception", Year = "2005"},
                new MovieDto { Id = "3", Title = "Sniper", Year = "2007" },
                new MovieDto { Id = "4", Title = "Fast" , Year = "2005"},
            };
            _movieRepository = new Mock<IMovieRepository>();
            _movieRepository.Setup(r => r.GetAllMoviesAsync()).ReturnsAsync(movies);
            _movieRepository.Setup(r => r.GetMovieByIdAsync("1")).ReturnsAsync(movies[0]);
            _movieRepository.Setup(r => r.GetAllMoviesByYearAsync("2005")).ReturnsAsync(movies.Where(m => m.Year.Equals("2005")));

            _logger = new Mock<ILogger<MovieService>>();

            var mapper = new Mapper(new MapperConfiguration(c =>
            {
                c.AddProfile<AutoMapperProfile>();

            }));

            _fixture = new MovieService(mapper, _movieRepository.Object, _logger.Object);
        }

        [Test]
        public async Task FindAllAsync_ReturnsAllMovies()
        {
            var movies = await _fixture.FindAllAsync();
            Assert.AreEqual(4, movies.Count());
        }

        [Test]
        public async Task FindMovieByYear_WhenYearInvalid_ReturnNull()
        {
            var movie = await _fixture.FindAllMoviesByYearAsync("INVALID_YEAR");
            Assert.IsNull(movie);
        }


        [Test]
        public async Task FindMovieByYear_WhenYearValid_ReturnMovies()
        {
            var movies = await _fixture.FindAllMoviesByYearAsync("2005");
            Assert.IsNotNull(movies);
            Assert.AreEqual(2, movies.Count());
            Assert.IsNotNull(movies);
        }

        [Test]
        public async Task FindMovieById_WhenIdIsInvalid_ReturnNull()
        {
            var movie = await _fixture.FindMovieByIdAsync("INVALID_ID");
            Assert.IsNull(movie);
        }

        [Test]
        public async Task FindMovieById_WhenIdIsValid_ReturnMovie()
        {
            var movie = await _fixture.FindMovieByIdAsync("1");
            Assert.AreEqual("Encanto", movie.MovieTitle);
        }

        [Test]
        public async Task CreateMovieAsync_WhenMovieIsValid_ReturnCreated()
        {
            var newMovie = new MovieDto { Id = "5", Title = "Espresso Dica", Year = "2008" };
            _movieRepository
           .Setup(r => r.CreateMovieAsync(It.IsAny<MovieDto>()))
           .Returns(() => Task.FromResult(newMovie));

            var newMovieViewModel = new MovieViewModel { Title = "Espresso Dica", Year = "2008", Actors = "Shak", ImdbRating = "7.4" };


            var result = await _fixture.CreateMovieAsync(newMovieViewModel);
            Assert.AreEqual(CreateMovieStatus.Created, result.Status);
            Assert.AreEqual("Espresso Dica", result.Movie.MovieTitle);
        }

        [Test]
        public async Task CreateMovieAsync_WhenMovieIsInvalid_ReturnError()
        {
            _movieRepository
           .Setup(r => r.CreateMovieAsync(It.IsAny<MovieDto>()))
           .Returns(() => Task.FromResult<MovieDto>(null));

            var newMovieViewModel = new MovieViewModel { Title = "Espresso Dica", Year = "2008", Actors = "Shak", ImdbRating = "7.4" };


            var result = await _fixture.CreateMovieAsync(newMovieViewModel);
            Assert.AreEqual(CreateMovieStatus.Error, result.Status);
        }

        [Test]
        public async Task UpdateMovieAsync_WhenIdIsInvalid_ReturnNotFound()
        {
            var result = await _fixture.UpdateMovieAsync("INVALID_ID", new MovieViewModel());
            Assert.AreEqual(UpdateMovieStatus.NotFound, result.Status);
        }

        [Test]
        public async Task UpdateMovieAsync_WhenRepositoryReturnsNull_ReturnError()
        {
            _movieRepository
                .Setup(r => r.UpdateMovieAsync("1", It.IsAny<MovieDto>()))
                .Returns(() => Task.FromResult<MovieDto>(null));

            var result = await _fixture.UpdateMovieAsync("1", new MovieViewModel());
            Assert.AreEqual(UpdateMovieStatus.Error, result.Status);
        }

        [Test]
        public async Task UpdateMovieAsync_WhenRepositoryReturnsMovie_ReturnSuccess()
        {
            _movieRepository
                .Setup(r => r.UpdateMovieAsync("1", It.IsAny<MovieDto>()))
                .ReturnsAsync(new MovieDto());

            var result = await _fixture.UpdateMovieAsync("1", new MovieViewModel());
            Assert.AreEqual(UpdateMovieStatus.Updated, result.Status);
        }
    }
}