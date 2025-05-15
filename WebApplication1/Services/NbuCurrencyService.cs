using System.Globalization;
using System.Net.Http;
using System.Text.Json;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class NbuCurrencyService
    {
        private readonly HttpClient _httpClient;

        public NbuCurrencyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<NbuCurrencyRate>> GetRatesForPeriodAsync(string currencyCode, DateTime startDate, DateTime endDate)
        {
            var rates = new List<NbuCurrencyRate>();

            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                var urlDate = date.ToString("yyyyMMdd");
                var url = $"https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?valcode={currencyCode}&date={urlDate}&json";

                try
                {
                    var response = await _httpClient.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        var dayRates = JsonSerializer.Deserialize<List<NbuCurrencyRate>>(json);
                        if (dayRates != null && dayRates.Any())
                        {
                            rates.Add(dayRates.First());
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Помилка: {currencyCode} на {urlDate}: {ex.Message}");
                }

                await Task.Delay(100); // захист від перевантаження API
            }

            return rates.OrderBy(r => DateTime.ParseExact(r.Exchangedate, "dd.MM.yyyy", CultureInfo.InvariantCulture)).ToList();
        }
    }
}
