using System.Text.Json.Serialization;

namespace EmployeeManager.Models.Entity
{
    public class Login
    {
        [JsonPropertyName("email")]
        public required string Email { get; set; }
        [JsonPropertyName("password")]
        public required string Password { get; set; }
        [JsonPropertyName("twoFactorCode")]
        public  string TwoFactorCode { get; set; }
        [JsonPropertyName("twoFactorRecoveryCode")]
        public  string TwoFactorRecoveryCode { get; set; }
    }
}