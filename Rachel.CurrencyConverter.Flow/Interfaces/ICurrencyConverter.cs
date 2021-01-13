using System.Collections.Generic;

namespace Rachel.CurrencyConverter.Flow.Interfaces
{
    public interface ICurrencyConverter
    {
        IEnumerable<string> Currencies { get; }
        ConversionHistory History { get; }
        void LoadFile();
        decimal ConvertAmount(string fromCurrency, string toCurrency, decimal amount);
    }
}
