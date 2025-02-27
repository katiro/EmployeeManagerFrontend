using System.Text.Json.Serialization;

namespace EmployeeManager.Models.Entity
{
    public class Token
    {
        [JsonPropertyName("tokenType")]
        public string TokenType { get; set; }

        [JsonPropertyName("accessToken")]
        public string AccessToken { get; set; }

        [JsonPropertyName("expiresIn")]
        public int ExpiresIN { get; set; }
        
        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; }
    }
}