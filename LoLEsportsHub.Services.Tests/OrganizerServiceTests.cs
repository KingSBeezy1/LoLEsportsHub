using LoLEsportsHub.Data.Models;
using LoLEsportsHub.Data.Repository.Interfaces;
using LoLEsportsHub.Services.Core;
using LoLEsportsHub.Services.Core.Interfaces;
using MockQueryable;
using Moq;
using NUnit.Framework;
using System.Linq.Expressions;

namespace LoLEsportsHub.Services.Tests
{
    [TestFixture]
    public class OrganizerServiceTests
    {
        private Mock<IOrganizerRepository> _organizerRepositoryMock;
        private IOrganizerService _organizerService;

        [SetUp]
        public void Setup()
        {
            _organizerRepositoryMock = new Mock<IOrganizerRepository>(MockBehavior.Strict);
            _organizerService = new OrganizerService(_organizerRepositoryMock.Object);
        }
        [Test]
        public void PassAlways()
        {
            Assert.Pass();
        }

        [Test]
        public async Task GetIdByUserIdAsyncShouldReturnNullWithNullUserId()
        {
            Guid? organizerId = await _organizerService.GetIdByUserIdAsync(null);

            Assert.IsNull(organizerId);
        }

        [Test]
        public async Task GetIdByUserIdAsyncShouldReturnNullWithNonExistingUserId()
        {
            _organizerRepositoryMock
                .Setup(or => or.FirstOrDefaultAsync(It.IsAny<Expression<Func<Organizer, bool>>>()))
                .ReturnsAsync((Organizer?)null);

            Guid? organizerId = await _organizerService.GetIdByUserIdAsync("non-existing-user-id");

            Assert.IsNull(organizerId);
        }

        [Test]
        public async Task GetIdByUserIdAsyncShouldReturnOrganizerIdWithValidUserId()
        {
            
            var expectedTestOrganizerId = "d15a6f0c-a16c-487d-b536-c4948ba405aa";
            var testOrganizerUserId = "085f30e8-dd52-41f2-bb40-7c28e5d73aa7";
            var testOrganizer = new Organizer()
            {
                Id = Guid.Parse(expectedTestOrganizerId),
                IsDeleted = false,
                UserId = testOrganizerUserId,
                ManagedTournaments = new HashSet<Tournament>(),
            };

            _organizerRepositoryMock
                .Setup(or => or.FirstOrDefaultAsync(It.IsAny<Expression<Func<Organizer, bool>>>()))
                .ReturnsAsync(testOrganizer);

            
            Guid? organizerId = await _organizerService.GetIdByUserIdAsync(testOrganizerUserId);

            
            Assert.IsNotNull(organizerId);
            Assert.AreEqual(expectedTestOrganizerId.ToLower(), organizerId.ToString()!.ToLower());
        }

        [Test]
        public async Task ExistsByIdAsyncShouldReturnFalseWithNullId()
        {
            
            bool exists = await _organizerService.ExistsByIdAsync(null);

            
            Assert.IsFalse(exists);
        }

        [Test]
        public async Task ExistsByIdAsyncShouldReturnFalseWithNonExistingId()
        {
            
            var nonExistingOrganizerId = "218f0cfc-510b-4b2e-a5ec-2fbf1aad5827";
            var organizersList = new List<Organizer>
            {
                new Organizer()
                {
                    Id = Guid.Parse("527e5e28-7c7f-45e4-ba74-b3768f2ba26a"),
                    IsDeleted = false,
                    UserId = "ded63c7e-a7b5-4abb-868c-3ad116ecb139",
                    ManagedTournaments = new HashSet<Tournament>(),
                },
                new Organizer()
                {
                    Id = Guid.Parse("9e64c286-8efe-4e36-bbbe-49a188f47558"),
                    IsDeleted = false,
                    UserId = "75db3396-e799-4542-9358-87a91ab0e810",
                    ManagedTournaments = new HashSet<Tournament>(),
                }
            };

            var organizersQueryableMock = organizersList.BuildMock();

            _organizerRepositoryMock
                .Setup(or => or.GetAllAttached())
                .Returns(organizersQueryableMock);

            bool exists = await _organizerService.ExistsByIdAsync(nonExistingOrganizerId);

            Assert.IsFalse(exists);
        }

        [Test]
        public async Task ExistsByIdAsyncShouldReturnTrueWithValidId()
        {
            var organizersList = new List<Organizer>
            {
                new Organizer()
                {
                    Id = Guid.Parse("527e5e28-7c7f-45e4-ba74-b3768f2ba26a"),
                    IsDeleted = false,
                    UserId = "ded63c7e-a7b5-4abb-868c-3ad116ecb139",
                    ManagedTournaments = new HashSet<Tournament>(),
                },
                new Organizer()
                {
                    Id = Guid.Parse("9e64c286-8efe-4e36-bbbe-49a188f47558"),
                    IsDeleted = false,
                    UserId = "75db3396-e799-4542-9358-87a91ab0e810",
                    ManagedTournaments = new HashSet<Tournament>(),
                }
            };
            var existingOrganizerId = organizersList.First().Id.ToString();

            var organizersQueryableMock = organizersList.BuildMock();

            _organizerRepositoryMock
                .Setup(or => or.GetAllAttached())
                .Returns(organizersQueryableMock);

            bool exists = await _organizerService.ExistsByIdAsync(existingOrganizerId);

            Assert.IsTrue(exists);
        }

        [Test]
        public async Task ExistsByUserIdAsyncShouldReturnFalseWithNullUserId()
        {
            bool exists = await _organizerService.ExistsByUserIdAsync(null);

            Assert.IsFalse(exists);
        }

        [Test]
        public async Task ExistsByUserIdAsyncShouldReturnFalseWithNonExistingUserId()
        {
          
            var nonExistingUserId = "218f0cfc-510b-4b2e-a5ec-2fbf1aad5827";
            var organizersList = new List<Organizer>
            {
                new Organizer()
                {
                    Id = Guid.Parse("527e5e28-7c7f-45e4-ba74-b3768f2ba26a"),
                    IsDeleted = false,
                    UserId = "ded63c7e-a7b5-4abb-868c-3ad116ecb139",
                    ManagedTournaments = new HashSet<Tournament>(),
                },
                new Organizer()
                {
                    Id = Guid.Parse("9e64c286-8efe-4e36-bbbe-49a188f47558"),
                    IsDeleted = false,
                    UserId = "75db3396-e799-4542-9358-87a91ab0e810",
                    ManagedTournaments = new HashSet<Tournament>(),
                }
            };

            var organizersQueryableMock = organizersList.BuildMock();

            _organizerRepositoryMock
                .Setup(or => or.GetAllAttached())
                .Returns(organizersQueryableMock);

            bool exists = await _organizerService.ExistsByUserIdAsync(nonExistingUserId);

            Assert.IsFalse(exists);
        }

        [Test]
        public async Task ExistsByUserIdAsyncShouldReturnTrueWithValidUserId()
        {
            
            var organizersList = new List<Organizer>
            {
                new Organizer()
                {
                    Id = Guid.Parse("527e5e28-7c7f-45e4-ba74-b3768f2ba26a"),
                    IsDeleted = false,
                    UserId = "ded63c7e-a7b5-4abb-868c-3ad116ecb139",
                    ManagedTournaments = new HashSet<Tournament>(),
                },
                new Organizer()
                {
                    Id = Guid.Parse("9e64c286-8efe-4e36-bbbe-49a188f47558"),
                    IsDeleted = false,
                    UserId = "75db3396-e799-4542-9358-87a91ab0e810",
                    ManagedTournaments = new HashSet<Tournament>(),
                }
            };
            var existingUserId = organizersList.First().UserId;

            var organizersQueryableMock = organizersList.BuildMock();

            _organizerRepositoryMock
                .Setup(or => or.GetAllAttached())
                .Returns(organizersQueryableMock);

            bool exists = await _organizerService.ExistsByUserIdAsync(existingUserId);

            Assert.IsTrue(exists);
        }
    }
}