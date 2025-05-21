using System.Globalization;
using System.Text.Json;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class BtcService
    {
        private readonly HttpClient _httpClient;

        public BtcService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<BtcRate>> GetPricesForPeriodAsync(string coinId, DateTime startDate, DateTime endDate)
        {
            int days = (endDate - startDate).Days + 1;
            var url = $"https://api.coingecko.com/api/v3/coins/{coinId}/market_chart?vs_currency=uah&days={days}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);

            var grouped = new Dictionary<string, List<decimal>>();

            foreach (var item in doc.RootElement.GetProperty("prices").EnumerateArray())
            {
                var timestamp = item[0].GetDouble();
                var price = item[1].GetDecimal();

                var date = DateTimeOffset.FromUnixTimeMilliseconds((long)timestamp).DateTime.Date;
                if (date >= startDate && date <= endDate)
                {
                    var label = date.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
                    if (!grouped.ContainsKey(label))
                        grouped[label] = new List<decimal>();
                    grouped[label].Add(price);
                }
            }

            return grouped
                .OrderBy(g => DateTime.ParseExact(g.Key, "dd.MM.yyyy", CultureInfo.InvariantCulture))
                .Select(g => new BtcRate
                {
                    Date = g.Key,
                    Price = Math.Round(g.Value.Average(), 2)
                }).ToList();
        }
    }
}
