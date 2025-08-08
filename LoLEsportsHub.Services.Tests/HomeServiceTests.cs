using LoLEsportsHub.Data.Models;
using LoLEsportsHub.Data.Repository.Interfaces;
using LoLEsportsHub.Services.Core;
using LoLEsportsHub.Services.Core.Interfaces;
using LoLEsportsHub.Web.ViewModels.Home;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MockQueryable;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoLEsportsHub.Services.Tests
{
    [TestFixture]
    public class HomeServiceTests
    {
        private Mock<ITournamentRepository> _tournamentRepositoryMock;
        private Mock<ILogger<HomeService>> _loggerMock;
        private IHomeService _homeService;

        [SetUp]
        public void Setup()
        {
            _tournamentRepositoryMock = new Mock<ITournamentRepository>(MockBehavior.Strict);
            _loggerMock = new Mock<ILogger<HomeService>>();
            _homeService = new HomeService(
                _tournamentRepositoryMock.Object,
                _loggerMock.Object);
        }

        [Test]
        public async Task GetTrendingTournamentsAsyncShouldReturnEmptyListWhenNoTournamentsExist()
        {
            var emptyList = new List<Tournament>()
                .BuildMock();
            _tournamentRepositoryMock
                .Setup(tr => tr.GetAllAttached())
                .Returns(emptyList);

            var result = await _homeService.GetTrendingTournamentsAsync();

            Assert.IsEmpty(result);
        }

        [Test]
        public async Task GetTrendingTournamentsAsyncShouldReturnOnlyNonDeletedTournaments()
        {
            var testTournaments = new List<Tournament>
    {
        new Tournament { Id = Guid.NewGuid(), IsDeleted = false, Matches = new List<TournamentMatch>(3) },
        new Tournament { Id = Guid.NewGuid(), IsDeleted = true, Matches = new List<TournamentMatch>(5) },
        new Tournament { Id = Guid.NewGuid(), IsDeleted = false, Matches = new List<TournamentMatch>(2) }
    };

            var tournaments = testTournaments.BuildMock();
            var deletedTournamentId = testTournaments[1].Id.ToString();

            _tournamentRepositoryMock
                .Setup(tr => tr.GetAllAttached())
                .Returns(tournaments);

            var result = await _homeService.GetTrendingTournamentsAsync();

            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.All(t => !t.Id.Equals(deletedTournamentId)));
        }

        [Test]
        public async Task GetTrendingTournamentsAsyncShouldOrderByMatchCountDescending()
        {
            var matches1 = new List<TournamentMatch> { new TournamentMatch(), new TournamentMatch(), new TournamentMatch() };
            var matches2 = new List<TournamentMatch> { new TournamentMatch(), new TournamentMatch(), new TournamentMatch(),
                                                       new TournamentMatch(), new TournamentMatch() }; 
            var matches3 = new List<TournamentMatch> { new TournamentMatch(), new TournamentMatch() }; 

            var tournaments = new List<Tournament>
            {
                new Tournament { Id = Guid.NewGuid(), IsDeleted = false, Matches = matches1 },
                new Tournament { Id = Guid.NewGuid(), IsDeleted = false, Matches = matches2 },
                new Tournament { Id = Guid.NewGuid(), IsDeleted = false, Matches = matches3 }
            }.BuildMock();

            _tournamentRepositoryMock
                .Setup(tr => tr.GetAllAttached())
                .Returns(tournaments);

            var result = (await _homeService.GetTrendingTournamentsAsync()).ToList();

            Assert.AreEqual(5, result[0].MatchCount);
            Assert.AreEqual(3, result[1].MatchCount);
            Assert.AreEqual(2, result[2].MatchCount);
        }

        [Test]
        public async Task GetTrendingTournamentsAsyncShouldReturnMax6Tournaments()
        {
            var tournaments = new List<Tournament>();
            for (int i = 0; i < 10; i++)
            {
                tournaments.Add(new Tournament
                {
                    Id = Guid.NewGuid(),
                    IsDeleted = false,
                    Matches = new List<TournamentMatch>(i)
                });
            }
            var mockQueryable = tournaments
                .BuildMock();

            _tournamentRepositoryMock
                .Setup(tr => tr.GetAllAttached())
                .Returns(mockQueryable);

            var result = await _homeService.GetTrendingTournamentsAsync();

            Assert.AreEqual(6, result.Count());
        }

        [Test]
        public async Task GetTrendingTournamentsAsyncShouldReturnCorrectViewModel()
        {
            var testTournament = new Tournament
            {
                Id = Guid.NewGuid(),
                Name = "World Championship",
                Region = "Global",
                IsDeleted = false,
                Matches = new List<TournamentMatch>(5)
            };
            var tournaments = new List<Tournament> { testTournament }
            .BuildMock();

            _tournamentRepositoryMock
                .Setup(tr => tr.GetAllAttached())
                .Returns(tournaments);

            var result = (await _homeService.GetTrendingTournamentsAsync()).First();

            Assert.AreEqual(testTournament.Id.ToString(), result.Id);
            Assert.AreEqual(testTournament.Name, result.Name);
            Assert.AreEqual(testTournament.Region, result.Region);
            Assert.AreEqual(testTournament.Matches.Count, result.MatchCount);
        }

        [Test]
        public async Task GetTrendingTournamentsAsyncShouldHandleNullMatchesCollection()
        {
            var testTournament = new Tournament
            {
                Id = Guid.NewGuid(),
                Name = "Test Tournament",
                Region = "NA",
                IsDeleted = false,
                Matches = null
            };

            var mockTournaments = new List<Tournament> { testTournament }
                .BuildMock();

            _tournamentRepositoryMock
                .Setup(tr => tr.GetAllAttached())
                .Returns(mockTournaments);

            var result = await _homeService.GetTrendingTournamentsAsync();
            var firstResult = result.FirstOrDefault();

            Assert.IsNotNull(firstResult, "Expected one tournament but got none");
            Assert.AreEqual(0, firstResult.MatchCount);
        }
    }
}