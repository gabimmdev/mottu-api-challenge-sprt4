using Microsoft.ML.Data;

namespace MottuBackend.Models
{
    // Classe de entrada para o modelo ML.NET
    public class MotoData
    {
        [LoadColumn(0)]
        public float Cilindrada { get; set; } // Ex: 600
        
        [LoadColumn(1)]
        public float Potencia { get; set; } // Ex: 80
        
        [LoadColumn(2)]
        public float Peso { get; set; } // Ex: 180
        
        [LoadColumn(3)]
        [ColumnName("Label")]
        public bool AltaPerformance { get; set; } // True se for de alta performance
    }

    // Classe de saída para a predição do modelo
    public class MotoPrediction
    {
        [ColumnName("PredictedLabel")]
        public bool IsAltaPerformance { get; set; }
        
        public float Score { get; set; }
    }
}
