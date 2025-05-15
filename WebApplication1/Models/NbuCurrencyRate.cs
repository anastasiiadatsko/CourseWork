using System.Text.Json.Serialization;

namespace WebApplication1.Models
{
    public class NbuCurrencyRate
    {
        [JsonPropertyName("cc")]
        public string Cc { get; set; }

        [JsonPropertyName("rate")]
        public decimal Rate { get; set; }

        [JsonPropertyName("exchangedate")]
        public string Exchangedate { get; set; }
    }
}
