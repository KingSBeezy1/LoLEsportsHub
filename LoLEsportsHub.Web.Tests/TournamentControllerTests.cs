using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LoLEsportsHub.Services.Core.Interfaces;
using LoLEsportsHub.Web.ViewModels.Tournament;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace LoLEsportsHub.Controllers.Tests
{
    [TestFixture]
    public class TournamentControllerTests
    {
        private Mock<ITournamentService> _tournamentServiceMock;
        private TournamentController _controller;

        [SetUp]
        public void Setup()
        {
            _tournamentServiceMock = new Mock<ITournamentService>();
            _controller = new TournamentController(_tournamentServiceMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _controller = null; 
        }

        [Test]
        public async Task IndexShouldReturnViewWithTournamentsWhenServiceReturnsData()
        {
            var expectedTournaments = new List<UsersTournamentIndexViewModel>
            {
                new UsersTournamentIndexViewModel
                {
                    Id = "1",
                    Name = "World Championship",
                    Region = "Global"
                },
                new UsersTournamentIndexViewModel
                {
                    Id = "2",
                    Name = "Regional Finals",
                    Region = "NA"
                }
            };

            _tournamentServiceMock
                .Setup(s => s.GetAllTournamentsUserViewAsync())
                .ReturnsAsync(expectedTournaments);

            var result = await _controller.Index();

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.Model as IEnumerable<UsersTournamentIndexViewModel>;
            Assert.AreEqual(2, model.Count());
            Assert.AreEqual("World Championship", model.First().Name);
            Assert.AreEqual("Global", model.First().Region);
        }

        [Test]
        public async Task IndexShouldRedirectToHomeWhenExceptionOccurs()
        {
            _tournamentServiceMock
                .Setup(s => s.GetAllTournamentsUserViewAsync())
                .ThrowsAsync(new Exception("Test exception"));

            var result = await _controller.Index();

            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("Home", redirectResult.ControllerName);
            Assert.AreEqual("Index", redirectResult.ActionName);
        }
        [Test]
        public async Task BracketShouldReturnViewWithBracketDataWhenTournamentExists()
        {
            var expectedBracket = new TournamentBracketViewModel
            {
                TournamentName = "World Championship",
                OrganizerName = "Riot Games",
                Matches = new List<TournamentBracketMatchViewModel>
                {
                    new TournamentBracketMatchViewModel()
                }
            };

            _tournamentServiceMock
                .Setup(s => s.GetTournamentBracketAsync("1"))
                .ReturnsAsync(expectedBracket);

            var result = await _controller.Bracket("1");

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.Model as TournamentBracketViewModel;
            Assert.AreEqual("World Championship", model.TournamentName);
            Assert.AreEqual("Riot Games", model.OrganizerName);
            Assert.AreEqual(1, model.Matches.Count);
        }

        [Test]
        public async Task BracketShouldUseDefaultOrganizerNameWhenNotProvided()
        {
            var expectedBracket = new TournamentBracketViewModel
            {
                TournamentName = "World Championship",
                Matches = new List<TournamentBracketMatchViewModel>()
            };

            _tournamentServiceMock
                .Setup(s => s.GetTournamentBracketAsync("1"))
                .ReturnsAsync(expectedBracket);

            var result = await _controller.Bracket("1");

            var viewResult = result as ViewResult;
            var model = viewResult.Model as TournamentBracketViewModel;
            Assert.AreEqual("Unknown", model.OrganizerName);
        }

        [Test]
        public async Task BracketShouldRedirectToIndexWhenTournamentNotFound()
        {
            _tournamentServiceMock
                .Setup(s => s.GetTournamentBracketAsync("1"))
                .ReturnsAsync((TournamentBracketViewModel?)null);

            var result = await _controller.Bracket("1");

            Assert.IsInstanceOf<RedirectToActionResult>(result);
        }
        [Test]
        public async Task DetailsShouldReturnViewWithDetailsWhenTournamentExists()
        {
            var expectedDetails = new TournamentDetailsViewModel
            {
                Name = "World Championship",
                Region = "Global",
                Matches = new List<TournamentDetailsMatchViewModel>
                {
                    new TournamentDetailsMatchViewModel()
                }
            };

            _tournamentServiceMock
                .Setup(s => s.GetTournamentDetailsAsync("1"))
                .ReturnsAsync(expectedDetails);

            var result = await _controller.Details("1");

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.Model as TournamentDetailsViewModel;
            Assert.AreEqual("World Championship", model.Name);
            Assert.AreEqual("Global", model.Region);
            Assert.AreEqual(1, model.Matches.Count());
        }

        [Test]
        public async Task DetailsShouldHandleEmptyMatchesList()
        {
            var expectedDetails = new TournamentDetailsViewModel
            {
                Name = "World Championship",
                Region = "Global",
                Matches = new List<TournamentDetailsMatchViewModel>()
            };

            _tournamentServiceMock
                .Setup(s => s.GetTournamentDetailsAsync("1"))
                .ReturnsAsync(expectedDetails);

            var result = await _controller.Details("1");

            var viewResult = result as ViewResult;
            var model = viewResult.Model as TournamentDetailsViewModel;
            Assert.IsEmpty(model.Matches);
        }
        [Test]
        public async Task BracketShouldHandleNullId()
        {
            var result = await _controller.Bracket(null);

            Assert.IsInstanceOf<RedirectToActionResult>(result);
        }

        [Test]
        public async Task DetailsShouldHandleNullId()
        {
            var result = await _controller.Details(null);

            Assert.IsInstanceOf<RedirectToActionResult>(result);
        }

        [Test]
        public async Task AllActionsShouldRedirectToIndexWhenExceptionOccurs()
        {
            _tournamentServiceMock
                .Setup(s => s.GetTournamentBracketAsync(It.IsAny<string>()))
                .ThrowsAsync(new Exception());
            _tournamentServiceMock
                .Setup(s => s.GetTournamentDetailsAsync(It.IsAny<string>()))
                .ThrowsAsync(new Exception());

            var bracketResult = await _controller.Bracket("1");
            var detailsResult = await _controller.Details("1");

            Assert.IsInstanceOf<RedirectToActionResult>(bracketResult);
            Assert.IsInstanceOf<RedirectToActionResult>(detailsResult);
        }
    }
}