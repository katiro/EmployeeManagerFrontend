using System.Text.Json.Serialization;

namespace EmployeeManager.Models.Entity
{
    public class Empleado
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("nombre")]
        public required string Nombre { get; set; }
        [JsonPropertyName("apellido")]
        public required string Apellido { get; set; }
        [JsonPropertyName("email")]
        public required string Email { get; set; }
        [JsonPropertyName("telefono")]
        public required string Telefono { get; set; }
        [JsonPropertyName("salario")]
        public decimal Salario { get; set; }
        [JsonPropertyName("fechaIngreso")]
        public DateTime FechaIngreso { get; set; }
    }
}