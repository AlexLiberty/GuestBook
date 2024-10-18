using GuestBook.Controllers;
using GuestBook.Models;
using GuestBook.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace xUnitTest
{
    public class AccountControllerTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly AccountController _controller;

        public AccountControllerTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _controller = new AccountController(_mockUserRepository.Object);
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
        }

        [Fact]
        public async Task LoginValidUserRedirectsToHome() 
        {
            // Arrange
            var model = new LoginModel { Email = "test@mail.com", Password = "password123" };
            var user = new User { Id = 2, Name = "Test User" };

            _mockUserRepository.Setup(repo => repo.AuthorizeUser(model.Email, model.Password)).ReturnsAsync(user);

            // Act
            var result = await _controller.Login(model) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Equal("Home", result.ControllerName);
        }

        [Fact]
        public async Task LoginInvalidUserReturnsViewWithError()
        {
            // Arrange
            var model = new LoginModel { Email = "test@example.com", Password = "wrongpassword" };

            _mockUserRepository.Setup(repo => repo.AuthorizeUser(model.Email, model.Password)).ReturnsAsync((User)null);

            // Act
            var result = await _controller.Login(model) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.False(_controller.ModelState.IsValid);
        }

        [Fact]
        public void LoginGetReturnsView()
        {
            // Act
            var result = _controller.Login() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Register_ValidModel_RedirectsToLogin()
        {
            // Arrange
            var model = new RegistrationModel { Email = "newuser@example.com", Password = "password123", ConfirmPassword = "password123", Name = "New User" };

            _mockUserRepository.Setup(repo => repo.UserExists(model.Email)).ReturnsAsync(false);
            _mockUserRepository.Setup(repo => repo.RegisterUser(model.Email, model.Name, model.Password)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Register(model) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Login", result.ActionName);
        }

        [Fact]
        public async Task Register_ExistingEmail_ReturnsViewWithError()
        {
            // Arrange
            var model = new RegistrationModel { Email = "existing@example.com", Password = "password123", ConfirmPassword = "password123", Name = "Existing User" };

            _mockUserRepository.Setup(repo => repo.UserExists(model.Email)).ReturnsAsync(true);

            // Act
            var result = await _controller.Register(model) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.False(_controller.ModelState.IsValid);
        }

        [Fact]
        public async Task Register_PasswordMismatch_ReturnsViewWithError()
        {
            // Arrange
            var model = new RegistrationModel { Email = "newuser@example.com", Password = "password123", ConfirmPassword = "password456", Name = "New User" };

            // Act
            var result = await _controller.Register(model) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.False(_controller.ModelState.IsValid);
            Assert.True(_controller.ModelState.ContainsKey(""));
        }

        [Fact]
        public void Logout_ClearsSession_RedirectsToHome()
        {
            // Arrange
            _controller.HttpContext.Session.SetString("UserId", "1");

            // Act
            var result = _controller.Logout() as RedirectToActionResult;

            // Assert
            Assert.Null(_controller.HttpContext.Session.GetString("UserId"));
            Assert.Equal("Index", result.ActionName);
            Assert.Equal("Home", result.ControllerName);
        }
    }
}