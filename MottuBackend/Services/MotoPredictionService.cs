using Microsoft.ML;
using MottuBackend.Models;
using System.Collections.Generic;
using System.Linq;

namespace MottuBackend.Services
{
    public class MotoPredictionService
    {
        private readonly MLContext _mlContext;
        private ITransformer _trainedModel;

        public MotoPredictionService()
        {
            _mlContext = new MLContext(seed: 0);
            TrainModel();
        }

        private void TrainModel()
        {
            // Dados de treinamento fictícios
            var trainingData = new List<MotoData>
            {
                // Baixa Performance
                new MotoData { Cilindrada = 125, Potencia = 10, Peso = 120, AltaPerformance = false },
                new MotoData { Cilindrada = 250, Potencia = 25, Peso = 150, AltaPerformance = false },
                new MotoData { Cilindrada = 300, Potencia = 30, Peso = 160, AltaPerformance = false },
                new MotoData { Cilindrada = 500, Potencia = 45, Peso = 180, AltaPerformance = false },
                
                // Alta Performance
                new MotoData { Cilindrada = 600, Potencia = 80, Peso = 180, AltaPerformance = true },
                new MotoData { Cilindrada = 750, Potencia = 110, Peso = 190, AltaPerformance = true },
                new MotoData { Cilindrada = 1000, Potencia = 150, Peso = 200, AltaPerformance = true },
                new MotoData { Cilindrada = 1200, Potencia = 180, Peso = 220, AltaPerformance = true },
                new MotoData { Cilindrada = 1300, Potencia = 200, Peso = 210, AltaPerformance = true }
            };

            IDataView dataView = _mlContext.Data.LoadFromEnumerable(trainingData);

            // Pipeline de treinamento
            var pipeline = _mlContext.Transforms.Concatenate("Features", "Cilindrada", "Potencia", "Peso")
                .Append(_mlContext.BinaryClassification.Trainers.FastTree(labelColumnName: "Label", featureColumnName: "Features"));

            // Treinar o modelo
            _trainedModel = pipeline.Fit(dataView);
        }

        public MotoPrediction Predict(MotoData input)
        {
            var predictionEngine = _mlContext.Model.CreatePredictionEngine<MotoData, MotoPrediction>(_trainedModel);
            return predictionEngine.Predict(input);
        }
    }
}
