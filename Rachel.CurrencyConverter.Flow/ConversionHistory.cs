using System;
using System.Collections.Generic;
using System.Text;

namespace Rachel.CurrencyConverter.Flow
{
    public class ConversionHistory
    {
        public List<ConversionResult> History { get; private set; } = new List<ConversionResult>();
    }
}
