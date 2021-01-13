using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Rachel.CurrencyConverter.Flow.Interfaces;
using Rachel.CurrencyConverter.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Rachel.CurrencyConverter.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICurrencyConverter _currencyConverter;

        public HomeController(ILogger<HomeController> logger, ICurrencyConverter currencyConverter)
        {
            _logger = logger;
            _currencyConverter = currencyConverter;
        }

        [HttpGet]
        public IActionResult Index()
        {
            HomeViewModel model = new HomeViewModel();
            if (_currencyConverter.Currencies.Count() == 0)
            {
                _currencyConverter.LoadFile();
            }
            _currencyConverter.Currencies.ToList().ForEach(c => model.Currencies.Add(new SelectListItem { Value = c, Text = c, Selected = c == "EUR" }));
            _currencyConverter.History.History.ForEach(cr => model.History.Add(cr));
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(string fromCurrency, string toCurrency, decimal amount)
        {
            decimal result = _currencyConverter.ConvertAmount(fromCurrency, toCurrency, amount);
            return new JsonResult(result);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
