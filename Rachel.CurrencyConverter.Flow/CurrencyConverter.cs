using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Rachel.CurrencyConverter.Flow.Config;
using Rachel.CurrencyConverter.Flow.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml;

namespace Rachel.CurrencyConverter.Flow
{
    public class CurrencyConverter : ICurrencyConverter
    {
        private static ConversionHistory _conversionHistory = new ConversionHistory();
        private static List<Currency> _currencies = new List<Currency>();
        private readonly CurrencyConverterConfig _config;

        public CurrencyConverter(IOptions<CurrencyConverterConfig> config)
        {
            _config = config.Value;
        }

        public IEnumerable<string> Currencies { get { return _currencies.Select(c => c.CurrencyId); } }

        public ConversionHistory History { get { return _conversionHistory; } }

        public void LoadFile() 
        {
            _currencies.Clear();
            
            using (WebClient client = new WebClient())
            {
                IWebProxy defaultProxy = WebRequest.DefaultWebProxy;
                if (defaultProxy != null)
                {
                    defaultProxy.Credentials = CredentialCache.DefaultCredentials;
                    client.Proxy = defaultProxy;
                }
                
                using (MemoryStream ms = new MemoryStream(client.DownloadData(_config.ExchangeRateFileUrl)))
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(ms);
                    XmlNodeList cubeNodes = xmlDocument.GetElementsByTagName("Cube");
                    XmlNode root = xmlDocument.FirstChild;
                    //TODO: cache and refresh if out of date
                    //XmlNodeList currencyNodes = root.SelectNodes("//Cube//Cube");
                    foreach (XmlNode node in cubeNodes)
                    {
                        if (node.Attributes["currency"] != null)
                        {
                            Currency currency = new Currency();
                            currency.CurrencyId = node.Attributes["currency"].Value;
                            currency.EurConversionRate = decimal.TryParse(node.Attributes["rate"].Value, out decimal parsedecimal) ? parsedecimal : 0;
                            if (currency.EurConversionRate != 0) { _currencies.Add(currency); }
                        }
                    }
                }
            }
              
            _currencies.Add(new Currency { CurrencyId = "EUR", EurConversionRate = 1 });

            //load hsitory
            if (File.Exists($"history\\conversion_history.json"))
            { 
                _conversionHistory = JsonConvert.DeserializeObject<ConversionHistory>(File.ReadAllText($"history\\conversion_history.json"));
            }
        }

        public decimal ConvertAmount(string fromCurrency, string toCurrency, decimal amount) 
        {
            if ((_currencies?.Count ?? 0) == 0)
            {
                LoadFile();
            }

            Currency from = _currencies.FirstOrDefault((c) => c.CurrencyId == fromCurrency);
            Currency to = _currencies.FirstOrDefault((c) => c.CurrencyId == toCurrency);
            //TODO: take into account not ofund currencies
            decimal result = (amount * from.EurConversionRate) * to.EurConversionRate;
            SaveConversionResult(fromCurrency, toCurrency, amount, result);
            return result;
        }
        private void SaveConversionResult(string fromCurrency, string toCurrency, decimal amount, decimal result)
        {
            _conversionHistory.History.Add(new ConversionResult
            {
                FromCurrency = fromCurrency,
                ToCurrency = toCurrency,
                Amount = amount,
                Result = result,
                ConversionDate = DateTime.Today.ToString()
            });

            if (!Directory.Exists("history"))
            {
                Directory.CreateDirectory("history");
            }
            using (StreamWriter sw = new StreamWriter($"history\\conversion_history.json"))
            { 
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(writer, _conversionHistory);
                }
            }
        }
    }
}
