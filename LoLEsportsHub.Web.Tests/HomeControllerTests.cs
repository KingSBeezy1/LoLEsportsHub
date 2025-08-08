using LoLEsportsHub.Controllers;
using LoLEsportsHub.Services.Core.Interfaces;
using LoLEsportsHub.Web.ViewModels.Home;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LoLEsportsHub.Tests.Controllers
{
    [TestFixture]
    public class HomeControllerTests
    {
        private Mock<ILogger<HomeController>> _mockLogger;
        private Mock<IHomeService> _mockHomeService;
        private HomeController _controller;

        [SetUp]
        public void Setup()
        {
            _mockLogger = new Mock<ILogger<HomeController>>();
            _mockHomeService = new Mock<IHomeService>();
            _controller = new HomeController(_mockLogger.Object, _mockHomeService.Object);
        }

        [Test]
        public async Task IndexReturnsViewResultWithHomePageViewModel()
        {
            var expectedTournaments = new List<HomeTournamentCardViewModel>
            {
                new HomeTournamentCardViewModel { Id = "1", Name = "Tournament 1" },
                new HomeTournamentCardViewModel { Id = "2", Name = "Tournament 2" }
            };

            _mockHomeService.Setup(s => s.GetTrendingTournamentsAsync())
                          .ReturnsAsync(expectedTournaments);

            var result = await _controller.Index();

            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.IsInstanceOf<HomePageViewModel>(viewResult.Model);

            var model = viewResult.Model as HomePageViewModel;
            Assert.AreEqual(2, model.TrendingTournaments.Count());
        }

        [Test]
        public async Task IndexWhenServiceReturnsEmptyListReturnsViewWithEmptyModel()
        {
            _mockHomeService.Setup(s => s.GetTrendingTournamentsAsync())
                          .ReturnsAsync(new List<HomeTournamentCardViewModel>());

            var result = await _controller.Index();

            var viewResult = result as ViewResult;
            var model = viewResult.Model as HomePageViewModel;
            Assert.IsEmpty(model.TrendingTournaments);
        }

        [Test]
        public async Task IndexWhenServiceThrowsExceptionReturnsViewWithEmptyModel()
        {
            var exception = new Exception("Test exception");
            _mockHomeService.Setup(s => s.GetTrendingTournamentsAsync())
                          .ThrowsAsync(exception);

            var result = await _controller.Index();

            var viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);

            var model = viewResult.Model as HomePageViewModel;
            Assert.IsNotNull(model);
            Assert.IsEmpty(model.TrendingTournaments);

            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) =>
                        o.ToString().Contains("Error getting trending tournaments")),
                    exception,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Test]
        public void IndexHasAllowAnonymousAttribute()
        {
            var methodInfo = typeof(HomeController).GetMethod("Index");

            var attributes = methodInfo.GetCustomAttributes(typeof(AllowAnonymousAttribute), true);

            Assert.IsNotEmpty(attributes);
        }

        [Test]
        public async Task IndexWhenUserAuthenticatedStillReturnsView()
        {
            var user = new System.Security.Claims.ClaimsPrincipal(
                new System.Security.Claims.ClaimsIdentity(
                    new System.Security.Claims.Claim[]
                    {
                        new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, "1"),
                        new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, "testuser")
                    },
                    "TestAuthentication"));

            _controller.ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext()
            {
                HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext() { User = user }
            };

            _mockHomeService.Setup(s => s.GetTrendingTournamentsAsync())
                          .ReturnsAsync(new List<HomeTournamentCardViewModel>());

            var result = await _controller.Index();

            Assert.IsInstanceOf<ViewResult>(result);
        }
    }
}