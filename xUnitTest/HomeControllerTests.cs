using GuestBook.Controllers;
using GuestBook.Models;
using GuestBook.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace xUnitTest
{
    public class HomeControllerTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IMessageRepository> _mockMessageRepository;
        private readonly HomeController _controller;

        public HomeControllerTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockMessageRepository = new Mock<IMessageRepository>();
            _controller = new HomeController(_mockUserRepository.Object, _mockMessageRepository.Object);
        }

        [Fact]
        public async Task IndexReturnsViewWithMessages()
        {
            // Arrange
            var messages = new List<Message>
        {
            new Message { Id = 1, MessageText = "Test message" }
        };

            _mockMessageRepository.Setup(repo => repo.GetMessagesWithUsers()).ReturnsAsync(messages);

            // Act
            var result = await _controller.Index() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(messages, result.Model);
        }

        [Fact]
        public async Task AddMessageValidUserAddsMessageAndRedirects()
        {
            // Arrange
            var userId = "1";
            var user = new User { Id = 1, Name = "Test User" };
            var messageText = "New message";

            _controller.HttpContext.Session.SetString("UserId", userId);

            _mockUserRepository.Setup(repo => repo.GetUserById(int.Parse(userId))).ReturnsAsync(user);

            // Act
            var result = await _controller.AddMessage(messageText) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _mockMessageRepository.Verify(repo => repo.AddMessage(It.IsAny<Message>()), Times.Once);
        }

        [Fact]
        public async Task AddMessageNoUserSessionRedirectsToIndexWithoutAddingMessage()
        {
            // Act
            var result = await _controller.AddMessage("Message text") as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            _mockMessageRepository.Verify(repo => repo.AddMessage(It.IsAny<Message>()), Times.Never);
        }
    }
}