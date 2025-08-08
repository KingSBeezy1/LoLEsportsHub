using LoLEsportsHub.Controllers;
using LoLEsportsHub.Data.Models;
using LoLEsportsHub.Services.Core.Interfaces;
using LoLEsportsHub.Web.ViewModels.Bookmarks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LoLEsportsHub.Tests.Controllers
{
    [TestFixture]
    public class BookmarksControllerTests
    {
        private Mock<IBookmarkService> _mockBookmarkService;
        private BookmarksController _controller;

        [SetUp]
        public void Setup()
        {
            _mockBookmarkService = new Mock<IBookmarkService>();
            _controller = new BookmarksController(_mockBookmarkService.Object);
        }

        private void SetAuthenticatedUser(string userId = "test-user")
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Name, "test@example.com")
            }, "mock"));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
        }

        #region Index Tests

        [Test]
        public async Task Index_ReturnsForbid_WhenUserNotAuthenticated()
        {
            // Arrange
            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = new ClaimsPrincipal() }
            };

            // Act
            var result = await _controller.Index();

            // Assert
            Assert.IsInstanceOf<ForbidResult>(result);
        }

        [Test]
        public async Task Index_ReturnsViewWithBookmarks_WhenUserAuthenticated()
        {
            // Arrange
            SetAuthenticatedUser();
            var expectedBookmarks = new List<BookmarkViewModel>
    {
        new BookmarkViewModel { MatchId = "1", Title = "Match 1" }
    };
            _mockBookmarkService.Setup(s => s.GetUserBookmarksAsync(It.IsAny<string>()))
                              .ReturnsAsync(expectedBookmarks);

            // Act
            var result = await _controller.Index();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;

            Assert.IsInstanceOf<IEnumerable<BookmarkViewModel>>(viewResult.Model);
            var model = viewResult.Model as IEnumerable<BookmarkViewModel>;

            Assert.AreEqual(1, model.Count());
        }

        [Test]
        public async Task Index_RedirectsToHome_WhenExceptionOccurs()
        {
            // Arrange
            SetAuthenticatedUser();
            _mockBookmarkService.Setup(s => s.GetUserBookmarksAsync(It.IsAny<string>()))
                              .ThrowsAsync(new Exception());

            // Act
            var result = await _controller.Index();

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("Index", redirectResult.ActionName);
            Assert.AreEqual("Home", redirectResult.ControllerName);
        }

        #endregion

        #region Add Tests

        [Test]
        public async Task Add_ReturnsForbid_WhenUserNotAuthenticated()
        {
            // Arrange
            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = new ClaimsPrincipal() }
            };

            // Act
            var result = await _controller.Add("match-1");

            // Assert
            Assert.IsInstanceOf<ForbidResult>(result);
        }

        [Test]
        public async Task Add_RedirectsToMatchIndex_WhenAddFails()
        {
            // Arrange
            SetAuthenticatedUser();
            _mockBookmarkService.Setup(s => s.AddMatchToUserBookmarksAsync(It.IsAny<string>(), It.IsAny<string>()))
                              .ReturnsAsync(false);

            // Act
            var result = await _controller.Add("match-1");

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("Index", redirectResult.ActionName);
            Assert.AreEqual("Match", redirectResult.ControllerName);
        }

        [Test]
        public async Task Add_RedirectsToBookmarksIndex_WhenAddSucceeds()
        {
            // Arrange
            SetAuthenticatedUser();
            _mockBookmarkService.Setup(s => s.AddMatchToUserBookmarksAsync(It.IsAny<string>(), It.IsAny<string>()))
                              .ReturnsAsync(true);

            // Act
            var result = await _controller.Add("match-1");

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("Index", redirectResult.ActionName);
            Assert.IsNull(redirectResult.ControllerName); // Same controller
        }

        [Test]
        public async Task Add_RedirectsToHome_WhenExceptionOccurs()
        {
            // Arrange
            SetAuthenticatedUser();
            _mockBookmarkService.Setup(s => s.AddMatchToUserBookmarksAsync(It.IsAny<string>(), It.IsAny<string>()))
                              .ThrowsAsync(new Exception());

            // Act
            var result = await _controller.Add("match-1");

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("Index", redirectResult.ActionName);
            Assert.AreEqual("Home", redirectResult.ControllerName);
        }

        #endregion

        #region Remove Tests

        [Test]
        public async Task Remove_ReturnsForbid_WhenUserNotAuthenticated()
        {
            // Arrange
            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = new ClaimsPrincipal() }
            };

            // Act
            var result = await _controller.Remove("match-1");

            // Assert
            Assert.IsInstanceOf<ForbidResult>(result);
        }

        [Test]
        public async Task Remove_RedirectsToBookmarksIndex_WhenRemoveFails()
        {
            // Arrange
            SetAuthenticatedUser();
            _mockBookmarkService.Setup(s => s.RemoveMatchFromBookmarksAsync(It.IsAny<string>(), It.IsAny<string>()))
                              .ReturnsAsync(false);

            // Act
            var result = await _controller.Remove("match-1");

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("Index", redirectResult.ActionName);
            Assert.IsNull(redirectResult.ControllerName); // Same controller
        }

        [Test]
        public async Task Remove_RedirectsToMatchIndex_WhenRemoveSucceeds()
        {
            // Arrange
            SetAuthenticatedUser();
            _mockBookmarkService.Setup(s => s.RemoveMatchFromBookmarksAsync(It.IsAny<string>(), It.IsAny<string>()))
                              .ReturnsAsync(true);

            // Act
            var result = await _controller.Remove("match-1");

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("Match", redirectResult.ControllerName);
        }

        [Test]
        public async Task Remove_RedirectsToHome_WhenExceptionOccurs()
        {
            // Arrange
            SetAuthenticatedUser();
            _mockBookmarkService.Setup(s => s.RemoveMatchFromBookmarksAsync(It.IsAny<string>(), It.IsAny<string>()))
                              .ThrowsAsync(new Exception());

            // Act
            var result = await _controller.Remove("match-1");

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("Index", redirectResult.ActionName);
            Assert.AreEqual("Home", redirectResult.ControllerName);
        }

        #endregion
    }
}