using System.ComponentModel.DataAnnotations;

namespace MottuBackend.Models
{
    public class Moto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Brand { get; set; }

        [Required]
        [MaxLength(100)]
        public string Model { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        [MaxLength(10)]
        public string LicensePlate { get; set; }
    }
}
