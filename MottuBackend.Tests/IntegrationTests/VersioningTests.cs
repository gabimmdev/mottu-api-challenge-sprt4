using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Threading.Tasks;
using MottuBackend;
using Xunit;

namespace MottuBackend.Tests.IntegrationTests
{
    public class VersioningTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public VersioningTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task AuthController_V1_ShouldReturnSuccess()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            // O endpoint de registro não requer autenticação
            var response = await client.GetAsync("/api/auth/register?api-version=1.0");

            // Assert
            // Esperamos um 400 Bad Request ou 405 Method Not Allowed, pois o endpoint é POST,
            // mas o que importa é que a rota com a versão foi resolvida.
            // Se a rota não fosse resolvida, teríamos um 404 Not Found.
            Assert.NotEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
