using Microsoft.AspNetCore.Mvc;
using WebApplication1.Services;
using Microsoft.AspNetCore.Identity;
using WebApplication1.Models;
using System.Globalization;

public class HomeController : Controller
{
    private readonly NbuCurrencyService _nbuService;
    private readonly BtcService _btcService;
    private readonly UserManager<ApplicationUser> _userManager;

    public HomeController(NbuCurrencyService nbuService, BtcService btcService, UserManager<ApplicationUser> userManager)
    {
        _nbuService = nbuService;
        _btcService = btcService;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var endDate = DateTime.Today;
        var startDate = endDate.AddDays(-6);

        ViewBag.WeekOptions = null;
        ViewBag.SelectedWeek = "";

        if (User.Identity?.IsAuthenticated ?? false)
        {
            var user = await _userManager.GetUserAsync(User);
            ViewBag.UserName = user?.UserName ?? "користувачу";
        }

        ViewBag.Today = DateTime.Now.ToString("dd MMMM yyyy", new CultureInfo("uk-UA"));

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

        // --- Криптовалюти ---
        var btc = await _btcService.GetPricesForPeriodAsync("bitcoin", startDate, endDate);
        var eth = await _btcService.GetPricesForPeriodAsync("ethereum", startDate, endDate);
        var sol = await _btcService.GetPricesForPeriodAsync("solana", startDate, endDate);

        ViewBag.BtcLabels = btc.Select(x => x.Date).ToList();
        ViewBag.BtcValues = btc.Select(x => x.Price).ToList();

        ViewBag.EthLabels = eth.Select(x => x.Date).ToList();
        ViewBag.EthValues = eth.Select(x => x.Price).ToList();

        ViewBag.SolLabels = sol.Select(x => x.Date).ToList();
        ViewBag.SolValues = sol.Select(x => x.Price).ToList();

        return View();
    }
    public IActionResult Privacy()
    {
        return View();
    }

}
