using Microsoft.AspNetCore.Mvc;
using WebApplication1.Services;

public class HomeController : Controller
{
    private readonly NbuCurrencyService _nbuService;
    private readonly BtcService _btcService;

    public HomeController(NbuCurrencyService nbuService, BtcService btcService)
    {
        _nbuService = nbuService;
        _btcService = btcService;
    }

    public async Task<IActionResult> Index()
    {
        // Встановлюємо тижневий діапазон
        var endDate = DateTime.Today;
        var startDate = endDate.AddDays(-6);

        ViewBag.WeekOptions = null;
        ViewBag.SelectedWeek = "";

        // --- НБУ курси ---
        var usd = await _nbuService.GetRatesForPeriodAsync("USD", startDate, endDate);
        var eur = await _nbuService.GetRatesForPeriodAsync("EUR", startDate, endDate);
        var pln = await _nbuService.GetRatesForPeriodAsync("PLN", startDate, endDate);

        ViewBag.UsdLabels = usd.Select(x => x.Exchangedate).ToList();
        ViewBag.UsdValues = usd.Select(x => x.Rate).ToList();

        ViewBag.EurLabels = eur.Select(x => x.Exchangedate).ToList();
        ViewBag.EurValues = eur.Select(x => x.Rate).ToList();

        ViewBag.PlnLabels = pln.Select(x => x.Exchangedate).ToList();
        ViewBag.PlnValues = pln.Select(x => x.Rate).ToList();

        // --- BTC курс ---
        var btcData = await _btcService.GetPricesForPeriodAsync("bitcoin", startDate, endDate);

        ViewBag.BtcLabels = btcData.Select(x => x.Date).ToList();
        ViewBag.BtcValues = btcData.Select(x => x.Price).ToList();

        return View();
    }
}
