using LoLEsportsHub.Controllers;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace LoLEsportsHub.Tests.Controllers
{
    [TestFixture]
    public class OrganizerControllerTests
    {
        private OrganizerController _controller;

        [SetUp]
        public void Setup()
        {
            _controller = new OrganizerController();
        }

        [Test]
        public void IndexReturnsOkResult()
        {
            var result = _controller.Index();

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public void IndexReturnsCorrectMessage()
        {
            var result = _controller.Index() as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("I am the manager!", result.Value);
        }

        [Test]
        public void IndexWhenUserAuthenticatedStillReturnsOk()
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

            var result = _controller.Index();

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public void IndexWhenUserNotAuthenticateReturnsOk()
        {
            _controller.ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext()
            {
                HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext()
                {
                    User = new System.Security.Claims.ClaimsPrincipal()
                }
            };

            var result = _controller.Index();

            Assert.IsInstanceOf<OkObjectResult>(result);
        }
    }
}