using Microsoft.AspNetCore.Mvc;
using MottuBackend.Models;
using MottuBackend.Services;
using Microsoft.AspNetCore.Authorization;

namespace MottuBackend.Controllers
{
    [Authorize]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class MotoPredictionController : ControllerBase
    {
        private readonly MotoPredictionService _predictionService;

        public MotoPredictionController(MotoPredictionService predictionService)
        {
            _predictionService = predictionService;
        }

        /// <summary>
        /// Prevê se uma moto é de alta performance com base em seus atributos.
        /// </summary>
        /// <param name="data">Dados da moto para predição.</param>
        /// <returns>Resultado da predição.</returns>
        [HttpPost("predict")]
        public IActionResult Predict([FromBody] MotoData data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var prediction = _predictionService.Predict(data);

            var result = new
            {
                Cilindrada = data.Cilindrada,
                Potencia = data.Potencia,
                Peso = data.Peso,
                IsAltaPerformance = prediction.IsAltaPerformance,
                Score = prediction.Score
            };

            return Ok(result);
        }
    }
}
