using LoLEsportsHub.Controllers;
using LoLEsportsHub.Services.Core.Interfaces;
using LoLEsportsHub.Web.ViewModels.Match;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LoLEsportsHub.Tests.Controllers
{
    [TestFixture]
    public class MatchControllerTests
    {
        private Mock<IMatchService> _mockMatchService;
        private Mock<IBookmarkService> _mockBookmarkService;
        private MatchController _controller;

        [SetUp]
        public void Setup()
        {
            _mockMatchService = new Mock<IMatchService>();
            _mockBookmarkService = new Mock<IBookmarkService>();
            _controller = new MatchController(_mockMatchService.Object, _mockBookmarkService.Object);
        }

        [Test]
        public async Task IndexReturnsViewResultWithListOfMatches()
        {
            // Arrange
            var matches = new List<AllMatchesIndexViewModel>
            {
                new AllMatchesIndexViewModel { Id = "1", Title = "Match 1" },
                new AllMatchesIndexViewModel { Id = "2", Title = "Match 2" }
            };

            _mockMatchService.Setup(s => s.GetAllMatchesAsync()).ReturnsAsync(matches);

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = new ClaimsPrincipal() }
            };

            var result = await _controller.Index();

            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.IsInstanceOf<IEnumerable<AllMatchesIndexViewModel>>(viewResult.Model);
            Assert.AreEqual(2, (viewResult.Model as IEnumerable<AllMatchesIndexViewModel>).Count());
        }

        [Test]
        public async Task IndexWhenUserAuthenticatedSetsBookmarkStatus()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "user1"),
                new Claim(ClaimTypes.Name, "testuser")
            }, "mock"));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            var matches = new List<AllMatchesIndexViewModel>
            {
                new AllMatchesIndexViewModel { Id = "1", Title = "Match 1" }
            };

            _mockMatchService.Setup(s => s.GetAllMatchesAsync()).ReturnsAsync(matches);
            _mockBookmarkService.Setup(s => s.IsMatchAddedToBookmarks("1", "user1")).ReturnsAsync(true);

            var result = await _controller.Index();

            var viewResult = result as ViewResult;
            var model = viewResult.Model as IEnumerable<AllMatchesIndexViewModel>;
            Assert.IsTrue(model.First().IsAddedToUserBookmarks);
        }

        [Test]
        public async Task AddGetReturnsViewResult()
        {
            var result = await _controller.Add();

            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task AddPostInvalidModelReturnsViewWithModel()
        {
            var model = new MatchFormInputModel();
            _controller.ModelState.AddModelError("Title", "Required");

            var result = await _controller.Add(model);

            var viewResult = result as ViewResult;
            Assert.AreEqual(model, viewResult.Model);
        }

        [Test]
        public async Task Add_PostValidModelRedirectsToIndex()
        {
            var model = new MatchFormInputModel { Title = "Test", Region = "NA", ScheduledDate = "2023-01-01" };

            var result = await _controller.Add(model);

            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("Index", redirectResult.ActionName);
        }

        [Test]
        public async Task AddPostThrowsExceptionReturnsViewWithError()
        {
            var model = new MatchFormInputModel { Title = "Test" };
            _mockMatchService.Setup(s => s.AddMatchAsync(model)).ThrowsAsync(new Exception());

            var result = await _controller.Add(model);

            var viewResult = result as ViewResult;
            Assert.Greater(_controller.ModelState.ErrorCount, 0);
        }

        [Test]
        public async Task DetailsWithValidIdReturnsViewWithModel()
        {
            var match = new MatchDetailsViewModel { Id = "1", Title = "Test" };
            _mockMatchService.Setup(s => s.GetMatchDetailsByIdAsync("1")).ReturnsAsync(match);

            var result = await _controller.Details("1");

            var viewResult = result as ViewResult;
            Assert.AreEqual(match, viewResult.Model);
        }

        [Test]
        public async Task DetailsWithNullIdRedirectsToIndex()
        {
            var result = await _controller.Details(null);

            Assert.IsInstanceOf<RedirectToActionResult>(result);
        }

        [Test]
        public async Task DetailsWithNonexistentIdRedirectsToIndex()
        {
            _mockMatchService.Setup(s => s.GetMatchDetailsByIdAsync("999")).ReturnsAsync((MatchDetailsViewModel)null);

            var result = await _controller.Details("999");

            Assert.IsInstanceOf<RedirectToActionResult>(result);
        }

        [Test]
        public async Task DetailsThrowsExceptionRedirectsToIndex()
        {
            _mockMatchService.Setup(s => s.GetMatchDetailsByIdAsync("1")).ThrowsAsync(new Exception());

            var result = await _controller.Details("1");

            Assert.IsInstanceOf<RedirectToActionResult>(result);
        }


        [Test]
        public async Task DetailsPartialWithValidIdReturnsPartialView()
        {
            var match = new MatchDetailsViewModel { Id = "1", Title = "Test" };
            _mockMatchService.Setup(s => s.GetMatchDetailsByIdAsync("1")).ReturnsAsync(match);

            var result = await _controller.DetailsPartial("1");

            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreEqual("_MatchDetailsPartial", viewResult.ViewName);
        }

        [Test]
        public async Task EditGetWithValidIdReturnsViewWithModel()
        {
            var match = new MatchFormInputModel { Id = "1", Title = "Test" };
            _mockMatchService.Setup(s => s.GetEditableMatchByIdAsync("1")).ReturnsAsync(match);

            var result = await _controller.Edit("1");

            var viewResult = result as ViewResult;
            Assert.AreEqual(match, viewResult.Model);
        }

        [Test]
        public async Task EditGetWithNonexistentIdRedirectsToIndex()
        {
            _mockMatchService.Setup(s => s.GetEditableMatchByIdAsync("999")).ReturnsAsync((MatchFormInputModel)null);

            var result = await _controller.Edit("999");

            Assert.IsInstanceOf<RedirectToActionResult>(result);
        }

        [Test]
        public async Task EditPostInvalidModelReturnsViewWithModel()
        {
            var model = new MatchFormInputModel();
            _controller.ModelState.AddModelError("Title", "Required");

            var result = await _controller.Edit(model);

            var viewResult = result as ViewResult;
            Assert.AreEqual(model, viewResult.Model);
        }

        [Test]
        public async Task EditPostValidModelRedirectsToDetails()
        {
            var model = new MatchFormInputModel { Id = "1", Title = "Test" };
            _mockMatchService.Setup(s => s.EditMatchAsync(model)).ReturnsAsync(true);

            var result = await _controller.Edit(model);

            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("Details", redirectResult.ActionName);
            Assert.AreEqual("1", redirectResult.RouteValues["id"]);
        }


        [Test]
        public async Task DeleteGetWithValidIdReturnsViewWithModel()
        {
            var match = new DeleteMatchViewModel { Id = "1", Title = "Test" };
            _mockMatchService.Setup(s => s.GetMatchDeleteDetailsByIdAsync("1")).ReturnsAsync(match);

            var result = await _controller.Delete("1");

            var viewResult = result as ViewResult;
            Assert.AreEqual(match, viewResult.Model);
        }

        [Test]
        public async Task DeletePostValidModelRedirectsToIndex()
        {
            var model = new DeleteMatchViewModel { Id = "1" };
            _mockMatchService.Setup(s => s.SoftDeleteMatchAsync("1")).ReturnsAsync(true);

            var result = await _controller.Delete(model);

            Assert.IsInstanceOf<RedirectToActionResult>(result);
        }

        [Test]
        public async Task DeletePostInvalidModelRedirectsToIndex()
        {
            var model = new DeleteMatchViewModel();
            _controller.ModelState.AddModelError("Id", "Required");

            var result = await _controller.Delete(model);

            Assert.IsInstanceOf<RedirectToActionResult>(result);
        }
    }
}