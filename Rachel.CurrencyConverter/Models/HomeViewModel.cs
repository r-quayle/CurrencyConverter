using Microsoft.AspNetCore.Mvc.Rendering;
using Rachel.CurrencyConverter.Flow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rachel.CurrencyConverter.Models
{
    public class HomeViewModel
    {
        public List<SelectListItem> Currencies { get; } = new List<SelectListItem>();
        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }
        public decimal FromAmount { get; set; }
        public decimal ToAmount { get; }
        public List<ConversionResult> History { get; } = new List<ConversionResult>();
    }
}
