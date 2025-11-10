using Xunit;
using MottuBackend.Controllers;
using MottuBackend.Data;
using MottuBackend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace MottuBackend.Tests.UnitTests
{
    public class AuthControllerTests
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            // Configuração do DbContext em memória
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            _context = new AppDbContext(options);
            _context.Database.EnsureDeleted(); // Limpa o banco antes de cada teste

            // Configuração do IConfiguration para JWT
            var inMemorySettings = new Dictionary<string, string> {
                {"Jwt:Key", "supersecretkeythatisatleast16characterslong"},
                {"Jwt:Issuer", "TestIssuer"},
                {"Jwt:Audience", "TestAudience"}
            };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _controller = new AuthController(_context, _configuration);
        }

        [Fact]
        public async Task Register_ShouldReturnOk_WhenUserIsNew()
        {
            // Arrange
            var request = new UserRegistrationDto { Username = "newUser", Password = "password123" };

            // Act
            var result = await _controller.Register(request);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var userExists = await _context.Users.AnyAsync(u => u.Username == "newUser");
            Assert.True(userExists);
        }

        [Fact]
        public async Task Register_ShouldReturnBadRequest_WhenUserAlreadyExists()
        {
            // Arrange
            var existingUser = new User { Username = "existingUser", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123") };
            _context.Users.Add(existingUser);
            await _context.SaveChangesAsync();
            var request = new UserRegistrationDto { Username = "existingUser", Password = "newPassword" };

            // Act
            var result = await _controller.Register(request);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Login_ShouldReturnOkWithToken_WhenCredentialsAreValid()
        {
            // Arrange
            var password = "validPassword";
            var user = new User { Username = "validUser", PasswordHash = BCrypt.Net.BCrypt.HashPassword(password) };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            var request = new UserLoginDto { Username = "validUser", Password = password };

            // Act
            var result = await _controller.Login(request);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            var tokenObject = okResult.Value as dynamic;
            Assert.NotNull(tokenObject.Token);
        }

        [Fact]
        public async Task Login_ShouldReturnUnauthorized_WhenCredentialsAreInvalid()
        {
            // Arrange
            var password = "validPassword";
            var user = new User { Username = "validUser", PasswordHash = BCrypt.Net.BCrypt.HashPassword(password) };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            var request = new UserLoginDto { Username = "validUser", Password = "wrongPassword" };

            // Act
            var result = await _controller.Login(request);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(result);
        }
    }
}
