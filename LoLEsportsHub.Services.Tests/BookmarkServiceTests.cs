using LoLEsportsHub.Data.Models;
using LoLEsportsHub.Data.Repository.Interfaces;
using LoLEsportsHub.Services.Core;
using LoLEsportsHub.Services.Core.Interfaces;
using LoLEsportsHub.Web.ViewModels.Bookmarks;
using MockQueryable;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static LoLEsportsHub.GCommon.ApplicationConstants;
using Match = LoLEsportsHub.Data.Models.Match;

namespace LoLEsportsHub.Services.Tests
{
    [TestFixture]
    public class BookmarkServiceTests
    {
        private Mock<IBookmarkRepository> _bookmarkRepositoryMock;
        private IBookmarkService _bookmarkService;

        [SetUp]
        public void Setup()
        {
            _bookmarkRepositoryMock = new Mock<IBookmarkRepository>(MockBehavior.Strict);
            _bookmarkService = new BookmarkService(_bookmarkRepositoryMock.Object);
        }

        [Test]
        public async Task AddMatchToUserBookmarksAsyncShouldReturnFalseWhenMatchIdIsNull()
        {
            var result = await _bookmarkService.AddMatchToUserBookmarksAsync(null, "user1");
            Assert.IsFalse(result);
        }

        [Test]
        public async Task AddMatchToUserBookmarksAsyncShouldReturnFalseWhenUserIdIsNull()
        {
            var result = await _bookmarkService.AddMatchToUserBookmarksAsync("match1", null);
            Assert.IsFalse(result);
        }

        [Test]
        public async Task AddMatchToUserBookmarksAsyncShouldReturnFalseWhenMatchIdIsInvalid()
        {
            var result = await _bookmarkService.AddMatchToUserBookmarksAsync("invalid-guid", "user1");
            Assert.IsFalse(result);
        }

        [Test]
        public async Task AddMatchToUserBookmarksAsyncShouldUndeleteExistingBookmark()
        {
            var matchId = Guid.NewGuid().ToString();
            var userId = "user1";
            var existingBookmark = new ApplicationUserMatch
            {
                ApplicationUserId = userId,
                MatchId = Guid.Parse(matchId),
                IsDeleted = true
            };

            _bookmarkRepositoryMock
                .Setup(br => br.GetAllAttached())
                .Returns(new List<ApplicationUserMatch> { existingBookmark }.BuildMock());
            _bookmarkRepositoryMock
                .Setup(br => br.UpdateAsync(existingBookmark))
                .ReturnsAsync(true);

            var result = await _bookmarkService.AddMatchToUserBookmarksAsync(matchId, userId);
            Assert.IsTrue(result);
            Assert.IsFalse(existingBookmark.IsDeleted);
        }

        [Test]
        public async Task AddMatchToUserBookmarksAsyncShouldCreateNewBookmark()
        {
            var matchId = Guid.NewGuid().ToString();
            var userId = "user1";

            _bookmarkRepositoryMock
                .Setup(br => br.GetAllAttached())
                .Returns(new List<ApplicationUserMatch>().BuildMock());
            _bookmarkRepositoryMock
                .Setup(br => br.AddAsync(It.IsAny<ApplicationUserMatch>()))
                .Returns(Task.CompletedTask);

            var result = await _bookmarkService.AddMatchToUserBookmarksAsync(matchId, userId);
            Assert.IsTrue(result);
        }
        
        [Test]
        public async Task GetUserBookmarksAsyncShouldReturnEmptyListWhenNoBookmarksExist()
        {
            var emptyList = new List<ApplicationUserMatch>().BuildMock();
            _bookmarkRepositoryMock
                .Setup(br => br.GetAllAttached())
                .Returns(emptyList);

            var result = await _bookmarkService.GetUserBookmarksAsync("user1");
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task GetUserBookmarksAsyncShouldReturnOnlyUsersBookmarks()
        {
            var bookmarks = new List<ApplicationUserMatch>
            {
                new ApplicationUserMatch
                {
                    ApplicationUserId = "user1",
                    Match = new Match { Id = Guid.NewGuid(), Title = "Match1" }
                },
                new ApplicationUserMatch
                {
                    ApplicationUserId = "user2",
                    Match = new Match { Id = Guid.NewGuid(), Title = "Match2" }
                }
            }.BuildMock();

            _bookmarkRepositoryMock
                .Setup(br => br.GetAllAttached())
                .Returns(bookmarks);

            var result = await _bookmarkService.GetUserBookmarksAsync("user1");
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("Match1", result.First().Title);
        }

        [Test]
        public async Task GetUserBookmarksAsyncShouldReturnCorrectViewModel()
        {
            var testMatchId = Guid.NewGuid();
            var testMatch = new Match
            {
                Id = testMatchId,
                Title = "Test Match",
                Region = "NA",
                MatchDate = DateTime.Now,
                VODUrl = "http://test.com/vod"
            };

            var bookmark = new ApplicationUserMatch
            {
                ApplicationUserId = "user1",
                Match = testMatch,
                MatchId = testMatchId
            };

            var mockData = new List<ApplicationUserMatch> { bookmark };
            var mockQueryable = mockData.BuildMock();

            _bookmarkRepositoryMock
                .Setup(br => br.GetAllAttached())
                .Returns(mockQueryable);

            var result = (await _bookmarkService.GetUserBookmarksAsync("user1")).First();

            Assert.AreEqual(testMatchId.ToString(), result.MatchId);
            Assert.AreEqual(testMatch.Title, result.Title);
            Assert.AreEqual(testMatch.Region, result.Region);
            Assert.AreEqual(testMatch.MatchDate.ToString(AppDateFormat), result.ScheduleDate);
            Assert.AreEqual(testMatch.VODUrl, result.VodUrl);
        }

        [Test]
        public async Task IsMatchAddedToBookmarksShouldReturnFalseWhenMatchIdIsNull()
        {
            var result = await _bookmarkService.IsMatchAddedToBookmarks(null, "user1");
            Assert.IsFalse(result);
        }

        [Test]
        public async Task IsMatchAddedToBookmarksShouldReturnFalseWhenUserIdIsNull()
        {
            var result = await _bookmarkService.IsMatchAddedToBookmarks("match1", null);
            Assert.IsFalse(result);
        }

        [Test]
        public async Task IsMatchAddedToBookmarksShouldReturnFalseWhenMatchIdIsInvalid()
        {
            var result = await _bookmarkService.IsMatchAddedToBookmarks("invalid-guid", "user1");
            Assert.IsFalse(result);
        }

        [Test]
        public async Task IsMatchAddedToBookmarksShouldReturnTrueWhenBookmarkExists()
        {
            var matchId = Guid.NewGuid().ToString();
            var userId = "user1";
            var bookmark = new ApplicationUserMatch
            {
                ApplicationUserId = userId,
                MatchId = Guid.Parse(matchId)
            };

            _bookmarkRepositoryMock
                .Setup(br => br.SingleOrDefaultAsync(It.IsAny<Expression<Func<ApplicationUserMatch, bool>>>()))
                .ReturnsAsync(bookmark);

            var result = await _bookmarkService.IsMatchAddedToBookmarks(matchId, userId);
            Assert.IsTrue(result);
        }

        [Test]
        public async Task IsMatchAddedToBookmarksShouldReturnFalseWhenBookmarkDoesNotExist()
        {
            _bookmarkRepositoryMock
                .Setup(br => br.SingleOrDefaultAsync(It.IsAny<Expression<Func<ApplicationUserMatch, bool>>>()))
                .ReturnsAsync((ApplicationUserMatch?)null);

            var result = await _bookmarkService.IsMatchAddedToBookmarks(Guid.NewGuid().ToString(), "user1");
            Assert.IsFalse(result);
        }

        [Test]
        public async Task RemoveMatchFromBookmarksAsyncShouldReturnFalseWhenMatchIdIsNull()
        {
            var result = await _bookmarkService.RemoveMatchFromBookmarksAsync(null, "user1");
            Assert.IsFalse(result);
        }

        [Test]
        public async Task RemoveMatchFromBookmarksAsyncShouldReturnFalseWhenUserIdIsNull()
        {
            var result = await _bookmarkService.RemoveMatchFromBookmarksAsync("match1", null);
            Assert.IsFalse(result);
        }

        [Test]
        public async Task RemoveMatchFromBookmarksAsyncShouldReturnFalseWhenMatchIdIsInvalid()
        {
            var result = await _bookmarkService.RemoveMatchFromBookmarksAsync("invalid-guid", "user1");
            Assert.IsFalse(result);
        }

        [Test]
        public async Task RemoveMatchFromBookmarksAsyncShouldReturnTrueWhenBookmarkExists()
        {
            var matchId = Guid.NewGuid().ToString();
            var userId = "user1";
            var bookmark = new ApplicationUserMatch
            {
                ApplicationUserId = userId,
                MatchId = Guid.Parse(matchId)
            };

            _bookmarkRepositoryMock
                .Setup(br => br.SingleOrDefaultAsync(It.IsAny<Expression<Func<ApplicationUserMatch, bool>>>()))
                .ReturnsAsync(bookmark);
            _bookmarkRepositoryMock
                .Setup(br => br.DeleteAsync(bookmark))
                .ReturnsAsync(true);

            var result = await _bookmarkService.RemoveMatchFromBookmarksAsync(matchId, userId);
            Assert.IsTrue(result);
        }

        [Test]
        public async Task RemoveMatchFromBookmarksAsync_ShouldReturnFalse_WhenBookmarkDoesNotExist()
        {
            _bookmarkRepositoryMock
                .Setup(br => br.SingleOrDefaultAsync(It.IsAny<Expression<Func<ApplicationUserMatch, bool>>>()))
                .ReturnsAsync((ApplicationUserMatch?)null);

            var result = await _bookmarkService.RemoveMatchFromBookmarksAsync(Guid.NewGuid().ToString(), "user1");
            Assert.IsFalse(result);
        }
    }
}