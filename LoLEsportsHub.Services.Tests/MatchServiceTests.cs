using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoLEsportsHub.Data.Models;
using LoLEsportsHub.Data.Repository.Interfaces;
using LoLEsportsHub.Services.Core;
using LoLEsportsHub.Services.Core.Interfaces;
using LoLEsportsHub.Web.ViewModels.Match;
using MockQueryable;
using Moq;
using NUnit.Framework;
using static LoLEsportsHub.GCommon.ApplicationConstants;
using Match = LoLEsportsHub.Data.Models.Match;

namespace LoLEsportsHub.Services.Tests
{
    [TestFixture]
    public class MatchServiceTests
    {
        private Mock<IMatchRepository> _matchRepositoryMock;
        private IMatchService _matchService;

        [SetUp]
        public void Setup()
        {
            _matchRepositoryMock = new Mock<IMatchRepository>(MockBehavior.Strict);
            _matchService = new MatchService(_matchRepositoryMock.Object);
        }

        #region AddMatchAsync Tests
        [Test]
        public async Task AddMatchAsync_ShouldAddMatch_WithValidInput()
        {
            var inputModel = new MatchFormInputModel
            {
                Title = "Test Match",
                Region = "NA",
                VodUrl = "http://test.com/vod",
                ScheduledDate = "2023-12-31 15:00"
            };

            _matchRepositoryMock
                .Setup(mr => mr.AddAsync(It.IsAny<Match>()))
                .Returns(Task.CompletedTask);

            await _matchService.AddMatchAsync(inputModel);

            _matchRepositoryMock.Verify(mr => mr.AddAsync(It.Is<Match>(m =>
                m.Title == inputModel.Title &&
                m.Region == inputModel.Region &&
                m.VODUrl == inputModel.VodUrl &&
                m.MatchDate.ToString(AppDateFormat) == inputModel.ScheduledDate)),
                Times.Once);
        }

        [Test]
        public void AddMatchAsync_ShouldThrow_WithInvalidDateFormat()
        {
            var inputModel = new MatchFormInputModel
            {
                Title = "Test Match",
                Region = "NA",
                VodUrl = "http://test.com/vod",
                ScheduledDate = "invalid-date"
            };

            Assert.ThrowsAsync<FormatException>(() => _matchService.AddMatchAsync(inputModel));
        }
        #endregion

        #region DeleteMatchAsync Tests
        [Test]
        public async Task DeleteMatchAsync_ShouldReturnFalse_WithNullId()
        {
            var result = await _matchService.DeleteMatchAsync(null);
            Assert.IsFalse(result);
        }

        [Test]
        public async Task DeleteMatchAsync_ShouldReturnFalse_WithInvalidGuid()
        {
            var result = await _matchService.DeleteMatchAsync("invalid-guid");
            Assert.IsFalse(result);
        }

        [Test]
        public async Task DeleteMatchAsync_ShouldReturnFalse_WithNonExistingMatch()
        {
            var testId = Guid.NewGuid().ToString();
            _matchRepositoryMock
                .Setup(mr => mr.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Match?)null);

            var result = await _matchService.DeleteMatchAsync(testId);
            Assert.IsFalse(result);
        }

        [Test]
        public async Task DeleteMatchAsync_ShouldReturnTrue_WithExistingMatch()
        {
            var testMatch = new Match { Id = Guid.NewGuid() };
            _matchRepositoryMock
                .Setup(mr => mr.GetByIdAsync(testMatch.Id))
                .ReturnsAsync(testMatch);
            _matchRepositoryMock
                .Setup(mr => mr.HardDeleteAsync(testMatch))
                .ReturnsAsync(true);

            var result = await _matchService.DeleteMatchAsync(testMatch.Id.ToString());
            Assert.IsTrue(result);
        }
        #endregion

        #region EditMatchAsync Tests
        [Test]
        public async Task EditMatchAsync_ShouldReturnFalse_WithNullId()
        {
            var inputModel = new MatchFormInputModel { Id = null };
            var result = await _matchService.EditMatchAsync(inputModel);
            Assert.IsFalse(result);
        }

        [Test]
        public async Task EditMatchAsync_ShouldReturnFalse_WithInvalidGuid()
        {
            var inputModel = new MatchFormInputModel { Id = "invalid-guid" };
            var result = await _matchService.EditMatchAsync(inputModel);
            Assert.IsFalse(result);
        }

        [Test]
        public async Task EditMatchAsync_ShouldReturnFalse_WithNonExistingMatch()
        {
            var inputModel = new MatchFormInputModel
            {
                Id = Guid.NewGuid().ToString(),
                Title = "Updated Title",
                ScheduledDate = "2023-12-31 15:00"
            };
            _matchRepositoryMock
                .Setup(mr => mr.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Match?)null);

            var result = await _matchService.EditMatchAsync(inputModel);
            Assert.IsFalse(result);
        }

        [Test]
        public async Task EditMatchAsync_ShouldReturnTrue_WithValidInput()
        {
            var existingMatch = new Match
            {
                Id = Guid.NewGuid(),
                Title = "Original Title",
                MatchDate = DateTime.Now
            };
            var inputModel = new MatchFormInputModel
            {
                Id = existingMatch.Id.ToString(),
                Title = "Updated Title",
                Region = "Updated Region",
                VodUrl = "http://updated.com/vod",
                ScheduledDate = "2023-12-31 15:00"
            };
            _matchRepositoryMock
                .Setup(mr => mr.GetByIdAsync(existingMatch.Id))
                .ReturnsAsync(existingMatch);
            _matchRepositoryMock
                .Setup(mr => mr.UpdateAsync(It.IsAny<Match>()))
                .ReturnsAsync(true);

            var result = await _matchService.EditMatchAsync(inputModel);
            Assert.IsTrue(result);
            Assert.AreEqual(inputModel.Title, existingMatch.Title);
            Assert.AreEqual(inputModel.Region, existingMatch.Region);
            Assert.AreEqual(inputModel.VodUrl, existingMatch.VODUrl);
            Assert.AreEqual(inputModel.ScheduledDate, existingMatch.MatchDate.ToString(AppDateFormat));
        }
        #endregion

        #region GetAllMatchesAsync Tests
        [Test]
        public async Task GetAllMatchesAsync_ShouldReturnEmptyList_WhenNoMatchesExist()
        {
            var emptyList = new List<Match>()
                .BuildMock();
            _matchRepositoryMock
                .Setup(mr => mr.GetAllAttached())
                .Returns(emptyList);

            var result = await _matchService.GetAllMatchesAsync();
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task GetAllMatchesAsync_ShouldReturnMatches_WithDefaultVODUrl_WhenVODUrlIsEmpty()
        {
            var matches = new List<Match>
            {
                new Match { Id = Guid.NewGuid(), VODUrl = null },
                new Match { Id = Guid.NewGuid(), VODUrl = "" }
            }.BuildMock();

            _matchRepositoryMock
                .Setup(mr => mr.GetAllAttached())
                .Returns(matches);

            var result = await _matchService.GetAllMatchesAsync();
            Assert.IsTrue(result.All(m => m.VODurl == $"/images/{NoImageUrl}"));
        }

        [Test]
        public async Task GetAllMatchesAsync_ShouldReturnMatches_WithCorrectData()
        {
            var testDate = DateTime.Now;
            var matches = new List<Match>
            {
                new Match
                {
                    Id = Guid.NewGuid(),
                    Title = "Test Match 1",
                    Region = "NA",
                    MatchDate = testDate,
                    VODUrl = "http://test.com/vod1"
                },
                new Match
                {
                    Id = Guid.NewGuid(),
                    Title = "Test Match 2",
                    Region = "EU",
                    MatchDate = testDate.AddDays(1),
                    VODUrl = "http://test.com/vod2"
                }
            }.BuildMock();

            _matchRepositoryMock
                .Setup(mr => mr.GetAllAttached())
                .Returns(matches);

            var result = (await _matchService.GetAllMatchesAsync()).ToList();
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Test Match 1", result[0].Title);
            Assert.AreEqual("NA", result[0].Region);
            Assert.AreEqual(testDate.ToString(AppDateFormat), result[0].ScheduledDate);
        }
        #endregion

        #region GetEditableMatchByIdAsync Tests
        [Test]
        public async Task GetEditableMatchByIdAsync_ShouldReturnNull_WithNullId()
        {
            var result = await _matchService.GetEditableMatchByIdAsync(null);
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetEditableMatchByIdAsync_ShouldReturnNull_WithInvalidGuid()
        {
            var result = await _matchService.GetEditableMatchByIdAsync("invalid-guid");
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetEditableMatchByIdAsync_ShouldReturnNull_WithNonExistingMatch()
        {
            var testId = Guid.NewGuid().ToString();
            var emptyList = new List<Match>()
                .BuildMock();
            _matchRepositoryMock
                .Setup(mr => mr.GetAllAttached())
                .Returns(emptyList);

            var result = await _matchService.GetEditableMatchByIdAsync(testId);
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetEditableMatchByIdAsync_ShouldReturnMatch_WithValidId()
        {
            var testMatch = new Match
            {
                Id = Guid.NewGuid(),
                Title = "Test Match",
                Region = "NA",
                MatchDate = DateTime.Now,
                VODUrl = "http://test.com/vod"
            };
            var matches = new List<Match> { testMatch }
            .BuildMock();
            _matchRepositoryMock
                .Setup(mr => mr.GetAllAttached())
                .Returns(matches);

            var result = await _matchService.GetEditableMatchByIdAsync(testMatch.Id.ToString());
            Assert.IsNotNull(result);
            Assert.AreEqual(testMatch.Id.ToString(), result.Id);
            Assert.AreEqual(testMatch.Title, result.Title);
        }
        #endregion

        #region GetMatchDeleteDetailsByIdAsync Tests
        [Test]
        public async Task GetMatchDeleteDetailsByIdAsync_ShouldReturnNull_WithNullId()
        {
            var result = await _matchService.GetMatchDeleteDetailsByIdAsync(null);
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetMatchDeleteDetailsByIdAsync_ShouldReturnNull_WithInvalidGuid()
        {
            var result = await _matchService.GetMatchDeleteDetailsByIdAsync("invalid-guid");
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetMatchDeleteDetailsByIdAsync_ShouldReturnNull_WithNonExistingMatch()
        {
            var testId = Guid.NewGuid().ToString();
            _matchRepositoryMock
                .Setup(mr => mr.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Match?)null);

            var result = await _matchService.GetMatchDeleteDetailsByIdAsync(testId);
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetMatchDeleteDetailsByIdAsync_ShouldReturnDetails_WithExistingMatch()
        {
            var testMatch = new Match
            {
                Id = Guid.NewGuid(),
                Title = "Test Match",
                VODUrl = "http://test.com/vod"
            };
            _matchRepositoryMock
                .Setup(mr => mr.GetByIdAsync(testMatch.Id))
                .ReturnsAsync(testMatch);

            var result = await _matchService.GetMatchDeleteDetailsByIdAsync(testMatch.Id.ToString());
            Assert.IsNotNull(result);
            Assert.AreEqual(testMatch.Id.ToString(), result.Id);
            Assert.AreEqual(testMatch.VODUrl, result.VodUrl);
        }
        #endregion

        #region GetMatchDetailsByIdAsync Tests
        [Test]
        public async Task GetMatchDetailsByIdAsync_ShouldReturnNull_WithNullId()
        {
            var result = await _matchService.GetMatchDetailsByIdAsync(null);
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetMatchDetailsByIdAsync_ShouldReturnNull_WithInvalidGuid()
        {
            var result = await _matchService.GetMatchDetailsByIdAsync("invalid-guid");
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetMatchDetailsByIdAsync_ShouldReturnNull_WithNonExistingMatch()
        {
            var testId = Guid.NewGuid().ToString();
            var emptyList = new List<Match>()
                .BuildMock();
            _matchRepositoryMock
                .Setup(mr => mr.GetAllAttached())
                .Returns(emptyList);

            var result = await _matchService.GetMatchDetailsByIdAsync(testId);
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetMatchDetailsByIdAsync_ShouldReturnDetails_WithValidId()
        {
            var testMatch = new Match
            {
                Id = Guid.NewGuid(),
                Title = "Test Match",
                Region = "NA",
                MatchDate = DateTime.Now,
                VODUrl = "http://test.com/vod"
            };
            var matches = new List<Match> { testMatch }
                .BuildMock();
            _matchRepositoryMock
                .Setup(mr => mr.GetAllAttached())
                .Returns(matches);

            var result = await _matchService.GetMatchDetailsByIdAsync(testMatch.Id.ToString());
            Assert.IsNotNull(result);
            Assert.AreEqual(testMatch.Region, result.Region);
            Assert.AreEqual(testMatch.VODUrl, result.VodUrl);
        }
        #endregion

        #region SoftDeleteMatchAsync Tests
        [Test]
        public async Task SoftDeleteMatchAsync_ShouldReturnFalse_WithNullId()
        {
            var result = await _matchService.SoftDeleteMatchAsync(null);
            Assert.IsFalse(result);
        }

        [Test]
        public async Task SoftDeleteMatchAsync_ShouldReturnFalse_WithInvalidGuid()
        {
            var result = await _matchService.SoftDeleteMatchAsync("invalid-guid");
            Assert.IsFalse(result);
        }

        [Test]
        public async Task SoftDeleteMatchAsync_ShouldReturnFalse_WithNonExistingMatch()
        {
            var testId = Guid.NewGuid().ToString();
            _matchRepositoryMock
                .Setup(mr => mr.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Match?)null);

            var result = await _matchService.SoftDeleteMatchAsync(testId);
            Assert.IsFalse(result);
        }

        [Test]
        public async Task SoftDeleteMatchAsync_ShouldReturnTrue_WithExistingMatch()
        {
            var testMatch = new Match { Id = Guid.NewGuid() };
            _matchRepositoryMock
                .Setup(mr => mr.GetByIdAsync(testMatch.Id))
                .ReturnsAsync(testMatch);
            _matchRepositoryMock
                .Setup(mr => mr.DeleteAsync(testMatch))
                .ReturnsAsync(true);

            var result = await _matchService.SoftDeleteMatchAsync(testMatch.Id.ToString());
            Assert.IsTrue(result);
        }
        #endregion
    }
}