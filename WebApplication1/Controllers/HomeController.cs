using Microsoft.AspNetCore.Mvc;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly NbuCurrencyService _nbuService;

        public HomeController(NbuCurrencyService nbuService)
        {
            _nbuService = nbuService;
        }

        public async Task<IActionResult> Index(int weekOffset = 0)
        {
            // обчислюємо тижневі проміжки
            var today = DateTime.Today;
            var weekOptions = new List<(string Label, string Value)>();

            for (int i = 0; i < 6; i++)
            {
                var start = today.AddDays(-7 * (i + 1) + 1);
                var end = today.AddDays(-7 * i);
                string label = $"{start:dd.MM.yyyy} - {end:dd.MM.yyyy}";
                string value = i.ToString();
                weekOptions.Add((label, value));
            }

            ViewBag.WeekOptions = weekOptions;
            ViewBag.SelectedWeek = weekOffset.ToString();

            // вибраний діапазон дат
            var startDate = today.AddDays(-7 * (weekOffset + 1) + 1);
            var endDate = today.AddDays(-7 * weekOffset);

            var usd = await _nbuService.GetRatesForPeriodAsync("USD", startDate, endDate);
            var eur = await _nbuService.GetRatesForPeriodAsync("EUR", startDate, endDate);
            var pln = await _nbuService.GetRatesForPeriodAsync("PLN", startDate, endDate);

            ViewBag.UsdLabels = usd.Select(r => r.Exchangedate).ToList();
            ViewBag.UsdValues = usd.Select(r => r.Rate).ToList();

            ViewBag.EurLabels = eur.Select(r => r.Exchangedate).ToList();
            ViewBag.EurValues = eur.Select(r => r.Rate).ToList();

            ViewBag.PlnLabels = pln.Select(r => r.Exchangedate).ToList();
            ViewBag.PlnValues = pln.Select(r => r.Rate).ToList();

            return View();
        }
    }
}
