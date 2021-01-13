using System;
using System.Collections.Generic;
using System.Text;

namespace Rachel.CurrencyConverter.Flow
{
    public class ConversionResult
    {
        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }
        public decimal Amount { get; set; }
        public decimal Result { get; set; }
        public string ConversionDate { get; set; }
    }
}
