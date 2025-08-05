using LoLEsportsHub.Data.Models;
using LoLEsportsHub.Data.Repository.Interfaces;
using LoLEsportsHub.GCommon;
using LoLEsportsHub.Services.Core;
using LoLEsportsHub.Services.Core.Interfaces;
using LoLEsportsHub.Web.ViewModels.Tournament;
using Microsoft.EntityFrameworkCore;
using MockQueryable;
using Moq;
using Match = LoLEsportsHub.Data.Models.Match;

namespace LoLEsportsHub.Services.Tests
{
    [TestFixture]
    public class TournamentServiceTests
    {
        private Mock<ITournamentRepository> _mockTournamentRepository = null!;
        private TournamentService _tournamentService = null!;

        [SetUp]
        public void SetUp()
        {
            _mockTournamentRepository = new Mock<ITournamentRepository>();
            _tournamentService = new TournamentService(_mockTournamentRepository.Object);
        }

        [Test]
        public void PassAlways()
        {
            // Test that will always pass to show that the SetUp is working
            Assert.Pass();
        }

        [Test]
        public async Task GetAllTournamentsUserViewShouldReturnEmptyCollection()
        {
            List<Tournament> emptyTournamentList = new List<Tournament>();
            IQueryable<Tournament> emptyTournamentQueryable = emptyTournamentList
                .BuildMock();

            _mockTournamentRepository
                .Setup(r => r.GetAllAttached())
                .Returns(emptyTournamentQueryable);

            // Act
            IEnumerable<UsersTournamentIndexViewModel> emptyViewModelColl =
                await _tournamentService
                .GetAllTournamentsUserViewAsync();

            // Assert
            Assert.IsNotNull(emptyViewModelColl);
            Assert.AreEqual(emptyTournamentList.Count, emptyViewModelColl.Count());
        }
        [Test]
        public async Task GetAllTournamentsUserViewShouldReturnSameCollectionSizeWhenNonEmpty()
        {
            // Arrange
            List<Tournament> tournamentList = new List<Tournament>()
            {
                new Tournament
                {
                    Id = Guid.Parse("83388c3e-dd01-4268-b46a-e2351e464969"),
                    Name = "LEC Summer",
                    Region = "Europe",
                    IsDeleted = true,
                    OrganizerId = null,
                    Matches = new List<TournamentMatch>(),
                },
                new Tournament
                {
                    Id = Guid.Parse("83388c3e-dd01-4268-b46a-e3151e464969"),
                    Name = "LEC Summer",
                    Region = "Europe",
                    IsDeleted = true,
                    OrganizerId = null,
                    Matches = new List<TournamentMatch>(),
                }
            };
            IQueryable<Tournament> tournamentQueryable = tournamentList
                .BuildMock();

            _mockTournamentRepository
                .Setup(r => r.GetAllAttached())
                .Returns(tournamentQueryable);

            // Act
            IEnumerable<UsersTournamentIndexViewModel> result =
                await _tournamentService.GetAllTournamentsUserViewAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(tournamentList.Count, result.Count());
        }
        [Test]
        public async Task GetAllTournamentsUserViewShouldReturnSameDataInViewModels()
        {
            // Arrange
            List<Tournament> tournamentList = new List<Tournament>()
            {
                new Tournament
                {
                    Id = Guid.Parse("42388c3e-dd01-4268-b46a-e3151e464969"),
                    Name = "LEC Summer",
                    Region = "Europe",
                    IsDeleted = true,
                    OrganizerId = null,
                    Matches = new List<TournamentMatch>(),
                },
                new Tournament
                {
                    Id = Guid.Parse("82288c3e-dd01-4268-b46a-e3151e464969"),
                    Name = "LEC Summer",
                    Region = "Europe",
                    IsDeleted = true,
                    OrganizerId = null,
                    Matches = new List<TournamentMatch>(),
                }
            };
            IQueryable<Tournament> tournamentQueryable = tournamentList
                .BuildMock();

            _mockTournamentRepository
                .Setup(r => r.GetAllAttached())
                .Returns(tournamentQueryable);

            // Act
            IEnumerable<UsersTournamentIndexViewModel> result =
                await _tournamentService.GetAllTournamentsUserViewAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(tournamentList.Count, result.Count());

            foreach (Tournament tournament in tournamentList)
            {
                UsersTournamentIndexViewModel? vm = result
                    .FirstOrDefault(t => t.Id.ToLower() == tournament.Id.ToString().ToLower());

                Assert.IsNotNull(vm);
                Assert.AreEqual(tournament.Name, vm!.Name);
                Assert.AreEqual(tournament.Region, vm.Region);
            }
        }
        [Test]
        public async Task GetAllTournamentsUserViewShouldNotAddMoreDataThanPresent()
        {
            // Arrange
            List<Tournament> tournamentList = new List<Tournament>()
            {
                new Tournament
                {
                    Id = Guid.Parse("83348c3e-dd01-4268-b46a-e3155e464969"),
                    Name = "LEC Summer",
                    Region = "Europe",
                    IsDeleted = true,
                    OrganizerId = null,
                    Matches = new List<TournamentMatch>(),
                }
            };

            Tournament deletedTournament = new Tournament
            {
                Id = Guid.Parse("83388e3e-dd01-4268-b46a-e3151e464969"),
                Name = "LEC Summer",
                Region = "Europe",
                IsDeleted = true,
                OrganizerId = null,
                Matches = new List<TournamentMatch>(),
            };

            IQueryable<Tournament> tournamentQueryable = tournamentList
                .BuildMock();

            _mockTournamentRepository
                .Setup(r => r.GetAllAttached())
                .Returns(tournamentQueryable);

            // Act
            IEnumerable<UsersTournamentIndexViewModel> result =
                await _tournamentService.GetAllTournamentsUserViewAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(tournamentList.Count, result.Count());

            foreach (UsersTournamentIndexViewModel vm in result)
            {
                Assert.AreNotEqual(deletedTournament.Id.ToString().ToLower(), vm.Id.ToLower());
            }
        }
        [Test]
        public async Task GetTournamentBracketAsyncShouldReturnNullWithNullTournamentId()
        {
            TournamentBracketViewModel? tournamentVm = await this._tournamentService
                .GetTournamentBracketAsync(null);

            Assert.IsNull(tournamentVm);
        }

        [Test]
        public async Task GetTournamentBracketAsyncShouldReturnNullWithNonExistingTournamentId()
        {
            string nonExistingTournamentId = "018e23fa-4511-4ced-9532-bd2c200e57cb";
            List<Tournament> tournamentList = new List<Tournament>()
            {
                new Tournament()
                {
                    Id = Guid.Parse("50fc7855-3fc5-4c4c-a494-29eaa51e1035"),
                    Name = "LEC Summer",
                    Region = "Europe",
                    IsDeleted = true,
                    OrganizerId = null,
                    Matches = new List<TournamentMatch>(),
                },
                new Tournament()
                {
                   Id = Guid.Parse("83388c3e-dd01-4268-b46a-e3151e464969"),
                   Name = "LEC Summer",
                    Region = "Europe",
                    IsDeleted = true,
                    OrganizerId = null,
                    Matches = new List<TournamentMatch>(),
                },
            };
            IQueryable<Tournament> tournamentQueryable = tournamentList
                .BuildMock();

            this._mockTournamentRepository
                .Setup(cr => cr.GetAllAttached())
                .Returns(tournamentQueryable);

            TournamentBracketViewModel? cinemaVm = await this._tournamentService
                .GetTournamentBracketAsync(nonExistingTournamentId);

            Assert.IsNull(cinemaVm);
        }

        [Test]
        public async Task GetTournamentBracketAsyncShouldReturnViewModelHavingCorrespondingDataWithValidId()
        {
            List<Tournament> tournamentList = new List<Tournament>()
            {
                new Tournament()
                {
                    Id = Guid.Parse("50fc7855-3fc5-4c4c-a494-29eaa51e1035"),
                    Name = "LEC Summer",
                    Region = "Europe",
                    IsDeleted = true,
                    OrganizerId = null,
                    Matches = new List<TournamentMatch>(),
                },
                new Tournament()
                {
                   Id = Guid.Parse("83388c3e-dd01-4268-b46a-e3151e464969"),
                   Name = "LEC Summer",
                    Region = "Europe",
                    IsDeleted = true,
                    OrganizerId = null,
                    Matches = new List<TournamentMatch>(),
                },
            };
            Tournament searchedTournament = tournamentList.First();
            string expectedTournamentData = searchedTournament.Name + " - " + searchedTournament.Region;
            int expectedMatchesCount = 0;
            IQueryable<Tournament> tournamentQueryable = tournamentList
                .BuildMock();

            this._mockTournamentRepository
                .Setup(cr => cr.GetAllAttached())
                .Returns(tournamentQueryable);

            TournamentBracketViewModel? tournamentVm = await this._tournamentService
                .GetTournamentBracketAsync(searchedTournament.Id.ToString());

            Assert.IsNotNull(tournamentVm);
            Assert.AreEqual(searchedTournament.Name, tournamentVm!.TournamentName);

            Assert.IsNotNull(tournamentVm.Matches);
            Assert.AreEqual(expectedMatchesCount, tournamentVm.Matches.Count());
        }

        [Test]
        public async Task GetTournamentBracketAsyncShouldReturnViewModelHavingCorrespondingDataAndMatchesWithValidId()
        {
            Match matchInTournament = new Match()
            {
                Id = Guid.Parse("7915b191-3fa3-4419-bcf5-5e555fba4b52"),
                Title = "Random Match",
                VODUrl = "https://www.youtube.com/watch?v=QUCs2O_c8Xc&ab_channel=KazaLoLLCSHighlights",
                Region = "LCK"

            };
            List<TournamentMatch> tournamentMatches = new List<TournamentMatch>()
            {
                new TournamentMatch()
                {
                    Id = Guid.Parse("a2276de4-d7ef-48e8-9102-e14d3807c153"),
                    AvailableSlots = 50,
                    ScheduledTime = "19:45",
                    IsDeleted = false,
                    TournamentId = Guid.Parse("50fc7855-3fc5-4c4c-a494-29eaa51e1035"),
                    MatchId = Guid.Parse("7915b191-3fa3-4419-bcf5-5e555fba4b52"),
                    Match = matchInTournament,
                },
                new TournamentMatch()
                {
                    Id = Guid.Parse("7c2864a7-8e8c-4384-9bc6-2b8beb47a1ec"),
                    AvailableSlots = 50,
                    ScheduledTime = "19:45",
                    IsDeleted = false,
                    TournamentId = Guid.Parse("50fc2355-3fc5-4c4c-a494-29eaa51e1035"),
                    MatchId = Guid.Parse("7915b191-3fa3-4419-bcf5-5e555fba4b52"),
                    Match = matchInTournament,
                },
            };
            List<Tournament> tournamentsList = new List<Tournament>()
            {
                new Tournament()
                {
                    Id = Guid.Parse("50fc7855-3fc5-4c4c-a494-29eaa51e1035"),
                    Name = "LEC Summer",
                    Region = "Europe",
                    IsDeleted = true,
                    OrganizerId = null,
                    Matches = tournamentMatches
                        .Where(tm => tm.TournamentId == Guid.Parse("50fc7855-3fc5-4c4c-a494-29eaa51e1035"))
                        .ToList(),
                },
                new Tournament()
                {
                    Id = Guid.Parse("83388c3e-dd01-4268-b46a-e3151e464969"),
                    Name = "LEC Summer",
                    Region = "Korea",
                    IsDeleted = true,
                    OrganizerId = null,
                    Matches = tournamentMatches
                        .Where(tm => tm.TournamentId == Guid.Parse("83388c3e-dd01-4268-b46a-e3151e464969")) // or empty list
                        .ToList(),
                },

            };
            Tournament searchedTournament = tournamentsList.First();
            int expectedMoviesCount = 1;
            IQueryable<Tournament> tournamentQueryable = tournamentsList
                .BuildMock();

            this._mockTournamentRepository
                .Setup(cr => cr.GetAllAttached())
                .Returns(tournamentQueryable);

            TournamentBracketViewModel? tournamentVm = await this._tournamentService
                .GetTournamentBracketAsync(searchedTournament.Id.ToString());

            Assert.IsNotNull(tournamentVm);
            Assert.AreEqual(searchedTournament.Name, tournamentVm!.TournamentName);
            Assert.IsNotNull(tournamentVm.Matches);
            Assert.AreEqual(expectedMoviesCount, tournamentVm.Matches.Count());

            TournamentBracketMatchViewModel tournamentFirstMatch = tournamentVm.Matches
                .First();
            Assert.AreEqual(matchInTournament.Id.ToString().ToLower(), tournamentFirstMatch.Id.ToLower());
            Assert.AreEqual(matchInTournament.Title, tournamentFirstMatch.Title);
            Assert.AreEqual(matchInTournament.VODUrl, tournamentFirstMatch.VODUrl);
        }

        [Test]
        public async Task GetTournamentBracketAsyncShouldReturnViewModelHavingCorrespondingDataAndMatchesWithValidIdAndNullVODUrl()
        {
            
            Match matchInTournament = new Match()
            {
                Id = Guid.Parse("7915b191-3fa3-4419-bcf5-5e555fba4b52"),
                Title = "Random Match",
                VODUrl = null,
                Region = "LCK"
            };

            List<TournamentMatch> tournamentMatches = new List<TournamentMatch>()
    {
                new TournamentMatch()
                {
                    Id = Guid.Parse("a2276de4-d7ef-48e8-9102-e14d3807c153"),
                    AvailableSlots = 50,
                    ScheduledTime = "19:45",
                    IsDeleted = false,
                    TournamentId = Guid.Parse("50fc7855-3fc5-4c4c-a494-29eaa51e1035"),
                    MatchId = matchInTournament.Id,
                    Match = matchInTournament
                },
                new TournamentMatch()
                {
                    Id = Guid.Parse("7c2864a7-8e8c-4384-9bc6-2b8beb47a1ec"),
                    AvailableSlots = 55,
                    ScheduledTime = "21:45",
                    IsDeleted = false,
                    TournamentId = Guid.Parse("50fc7855-3fc5-4c4c-a494-29eaa51e1035"),
                    MatchId = matchInTournament.Id,
                    Match = matchInTournament
                },
            };

            List<Tournament> tournamentsList = new List<Tournament>()
    {
                new Tournament()
                {
                    Id = Guid.Parse("50fc7855-3fc5-4c4c-a494-29eaa51e1035"),
                    Name = "LEC Summer",
                    Region = "Europe",
                    IsDeleted = false,
                    OrganizerId = null,
                    Matches = tournamentMatches
                        .Where(tm => tm.TournamentId == Guid.Parse("50fc7855-3fc5-4c4c-a494-29eaa51e1035"))
                        .ToList()
                },
                new Tournament()
                {
                    Id = Guid.Parse("83388c3e-dd01-4268-b46a-e3151e464969"),
                    Name = "LEC Spring",
                    Region = "Korea",
                    IsDeleted = true,
                    OrganizerId = null,
                    Matches = new List<TournamentMatch>()
                }
            };

            Tournament searchedTournament = tournamentsList.First();
            string expectedTournamentData = searchedTournament.Name + " - " + searchedTournament.Region;
            int expectedMatchesCount = 1;
            string expectedVODFallback = $"/images/{ApplicationConstants.NoImageUrl}"; ; 

            IQueryable<Tournament> tournamentQueryable = tournamentsList.BuildMock();

            this._mockTournamentRepository
                .Setup(tr => tr.GetAllAttached())
                .Returns(tournamentQueryable);

            
            TournamentBracketViewModel? tournamentVm = await this._tournamentService
                .GetTournamentBracketAsync(searchedTournament.Id.ToString());

            
            Assert.IsNotNull(tournamentVm);
            Assert.AreEqual(searchedTournament.Name, tournamentVm!.TournamentName, "Tournament name should match");
            Assert.IsNotNull(tournamentVm.Matches);
            Assert.AreEqual(expectedMatchesCount, tournamentVm.Matches.Count(), "Should only include distinct matches");

            TournamentBracketMatchViewModel firstMatchVm = tournamentVm.Matches.First();
            Assert.AreEqual(matchInTournament.Id.ToString().ToLower(), firstMatchVm.Id.ToLower());
            Assert.AreEqual(matchInTournament.Title, firstMatchVm.Title);
            Assert.AreEqual(matchInTournament.Region, firstMatchVm.Region);

            Assert.IsNotNull(firstMatchVm.VODUrl, "VODUrl should not be null even if match VOD is null (fallback should be used)");
            Assert.AreEqual(expectedVODFallback, firstMatchVm.VODUrl);
        }

        [Test]
        public async Task GetTournamentDetailsAsyncShouldReturnNullWithNullTournamentId()
        {
            TournamentDetailsViewModel? cinemaVm = await this._tournamentService
                .GetTournamentDetailsAsync(null);

            Assert.IsNull(cinemaVm, "GetTournamentDetailsAsync should return null with null TournamentId!");
        }

        [Test]
        public async Task GetTournamentDetailsAsyncShouldReturnNullWithNonExistingTournamentId()
        {
           
            string nonExistingTournamentId = "018e23fa-4511-4ced-9532-bd2c200e57cb";

            List<Tournament> tournamentsList = new List<Tournament>()
    {
                new Tournament()
                {
                    Id = Guid.Parse("50fc7855-3fc5-4c4c-a494-29eaa51e1035"),
                    Name = "LEC Summer",
                    Region = "Europe",
                    IsDeleted = false,
                    OrganizerId = null,
                    Matches = new List<TournamentMatch>()
                },
                new Tournament()
                {
                    Id = Guid.Parse("83388c3e-dd01-4268-b46a-e3151e464969"),
                    Name = "LEC Spring",
                    Region = "Europe",
                    IsDeleted = true,
                    OrganizerId = null,
                    Matches = new List<TournamentMatch>()
                }
            };

            IQueryable<Tournament> tournamentQueryable = tournamentsList.BuildMock();

            this._mockTournamentRepository
                .Setup(tr => tr.GetAllAttached())
                .Returns(tournamentQueryable);

            
            TournamentDetailsViewModel? result = await this._tournamentService
                .GetTournamentDetailsAsync(nonExistingTournamentId);

            
            Assert.IsNull(result, "GetTournamentDetailsAsync should return null for a non-existing tournament ID.");
        }

        [Test]
        public async Task GetTournamentDetailsAsyncShouldReturnViewModelHavingCorrespondingDataWithValidId()
        {
            
            List<Tournament> tournamentsList = new List<Tournament>()
    {
                new Tournament()
                {
                    Id = Guid.Parse("50fc7855-3fc5-4c4c-a494-29eaa51e1035"),
                    Name = "LEC Summer",
                    Region = "Europe",
                    IsDeleted = false,
                    OrganizerId = null,
                    Matches = new List<TournamentMatch>()
                },
                new Tournament()
                {
                    Id = Guid.Parse("83388c3e-dd01-4268-b46a-e3151e464969"),
                    Name = "LEC Spring",
                    Region = "Europe",
                    IsDeleted = true,
                    OrganizerId = null,
                    Matches = new List<TournamentMatch>()
                }
    };

            Tournament searchedTournament = tournamentsList.First();
            int expectedMatchesCount = 0;

            IQueryable<Tournament> tournamentQueryable = tournamentsList.BuildMock();

            this._mockTournamentRepository
                .Setup(tr => tr.GetAllAttached())
                .Returns(tournamentQueryable);

           
            TournamentDetailsViewModel? result = await this._tournamentService
                .GetTournamentDetailsAsync(searchedTournament.Id.ToString());

            
            Assert.IsNotNull(result);
            Assert.AreEqual(searchedTournament.Name, result!.Name, "Tournament name should be copied to the ViewModel!");
            Assert.AreEqual(searchedTournament.Region, result.Region, "Tournament region should be copied to the ViewModel!");

            Assert.IsNotNull(result.Matches);
            Assert.AreEqual(expectedMatchesCount, result.Matches.Count(), "Tournament matches should have count 0 when no matches exist for the tournament.");
        }

        [Test]
        public async Task GetTournamentDetailsAsyncShouldReturnViewModelHavingCorrespondingDataAndMatchesWithValidId()
        {
            
            Match matchInTournament = new Match()
            {
                Id = Guid.Parse("7915b191-3fa3-4419-bcf5-5e555fba4b52"),
                Title = "Random Match",
                Region = "LCK",
                MatchDate = DateTime.UtcNow,
                VODUrl = "https://vodurl.com/match"
            };

            List<TournamentMatch> tournamentMatches = new List<TournamentMatch>()
            {
                new TournamentMatch()
                {
                    Id = Guid.Parse("a2276de4-d7ef-48e8-9102-e14d3807c153"),
                    AvailableSlots = 50,
                    ScheduledTime = "19:45",
                    IsDeleted = false,
                    TournamentId = Guid.Parse("50fc7855-3fc5-4c4c-a494-29eaa51e1035"),
                    MatchId = matchInTournament.Id,
                    Match = matchInTournament,
                },
                new TournamentMatch()
                {
                    Id = Guid.Parse("7c2864a7-8e8c-4384-9bc6-2b8beb47a1ec"),
                    AvailableSlots = 55,
                    ScheduledTime = "21:45",
                    IsDeleted = false,
                    TournamentId = Guid.Parse("50fc7855-3fc5-4c4c-a494-29eaa51e1035"),
                    MatchId = matchInTournament.Id,
                    Match = matchInTournament,
                },
            };

            List<Tournament> tournamentsList = new List<Tournament>()
            {
                new Tournament()
                {
                    Id = Guid.Parse("50fc7855-3fc5-4c4c-a494-29eaa51e1035"),
                    Name = "LEC Summer",
                    Region = "Europe",
                    IsDeleted = false,
                    OrganizerId = null,
                    Matches = tournamentMatches
                },
                new Tournament()
                {
                    Id = Guid.Parse("83388c3e-dd01-4268-b46a-e3151e464969"),
                    Name = "LEC Spring",
                    Region = "Europe",
                    IsDeleted = true,
                    OrganizerId = null,
                    Matches = new List<TournamentMatch>()
                },
            };

            Tournament searchedTournament = tournamentsList.First();
            int expectedMatchesCount = 1;

            IQueryable<Tournament> tournamentQueryable = tournamentsList.BuildMock();

            this._mockTournamentRepository
                .Setup(tr => tr.GetAllAttached())
                .Returns(tournamentQueryable);

            
            TournamentDetailsViewModel? result = await this._tournamentService
                .GetTournamentDetailsAsync(searchedTournament.Id.ToString());

            
            Assert.IsNotNull(result);
            Assert.AreEqual(searchedTournament.Name, result!.Name, "Tournament name should be copied to the ViewModel!");
            Assert.AreEqual(searchedTournament.Region, result.Region, "Tournament region should be copied to the ViewModel!");

            Assert.IsNotNull(result.Matches);
            Assert.AreEqual(expectedMatchesCount, result.Matches.Count(), "Tournament matches should take only distinct entries!");

            TournamentDetailsMatchViewModel firstMatchVm = result.Matches.First();

            Assert.AreEqual(matchInTournament.Title, firstMatchVm.Title);
            Assert.AreEqual(matchInTournament.MatchDate.ToString("yyyy-MM-dd HH:mm"), firstMatchVm.ScheduleDate);
        }
    }
}
