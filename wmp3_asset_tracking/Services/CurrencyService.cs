using System;
using System.Collections.Generic;
using System.Xml;
using wmp3_asset_tracking.Models;

namespace wmp3_asset_tracking.Services
{
    internal class CurrencyService
    {
        private static List<CurrencyObj> currencyList = new List<CurrencyObj>();

        // This method fetches the latest exchange rates from the ECB and populates the currencyList. If it fails (e.g., no internet), it seeds some default values.
        public static void FetchRates()
        {
            string url = "https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml";

            try
            {
                XmlTextReader reader = new XmlTextReader(url);
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        while (reader.MoveToNextAttribute())
                        {
                            if (reader.Name == "currency")
                            {
                                string currencyCode = reader.Value;
                                reader.MoveToNextAttribute();
                                double rate = double.Parse(reader.Value, System.Globalization.CultureInfo.InvariantCulture);
                                currencyList.Add(new CurrencyObj(currencyCode, rate));
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Fallback mechanism: if the user has no internet, seed some defaults
                currencyList.Clear();
                currencyList.Add(new CurrencyObj("USD", 1.08));
                currencyList.Add(new CurrencyObj("SEK", 11.40));
                currencyList.Add(new CurrencyObj("TRY", 35.20));
            }
        }

        // This method converts a price in USD to the local currency based on the office location
        public static decimal GetConvertedPrice(decimal priceUSD, OfficeLocation office)
        {
            // If rates haven't been fetched or failed, fetch them
            if (currencyList.Count == 0)
            {
                FetchRates();
            }

            string toCurrency = GetCurrency(office);
            if (toCurrency == "USD") 
            {
                return priceUSD; 
            }

            double input = (double)priceUSD;
            double usdRate = currencyList.Find(c => c.CurrencyCode == "USD")?.ExchangeRateFromEUR ?? 1.0;
            double targetRate = currencyList.Find(c => c.CurrencyCode == toCurrency)?.ExchangeRateFromEUR ?? 1.0;

            // USD to EUR, then EUR to Target Currency
            double value = (input / usdRate) * targetRate;

            return (decimal)value;
        }

        // This method is used to get the currency code for display purposes based on the office location
        public static string GetCurrency(OfficeLocation office)
        {
            switch (office)
            {
                case OfficeLocation.Sweden:
                    return "SEK";
                case OfficeLocation.USA:
                    return "USD";
                case OfficeLocation.Turkey:
                    return "TRY";
                default:
                    return "USD"; // Fallback if somehow unknown
            }
        }
    }
}



/*
 * OLD VERSION - This version had hardcoded exchange rates and did not attempt to fetch live data from the ECB. It also had a simpler structure without the CurrencyObj class or the FetchRates method.
 */


//using System;
//using System.Collections.Generic;
//using System.Text;
//using wmp3_asset_tracking.Models;

//namespace wmp3_asset_tracking.Services
//{
//    internal class CurrencyService
//    {
//        public static decimal GetExchangeRate(OfficeLocation office)
//        {
//            switch (office)
//            {
//                case OfficeLocation.Sweden:
//                    return 10.0m; // 1 USD = 10 SEK
//                case OfficeLocation.USA:
//                    return 1.0m; // 1 USD = 1 USD
//                case OfficeLocation.Turkey:
//                    return 32.0m;
//                default:
//                    return 1.0m; // Default to 1:1 if office is unknown
//            }
//        }

//        public static string GetCurrency(OfficeLocation office)
//        {
//            switch (office)
//            {
//                case OfficeLocation.Sweden:
//                    return "SEK"; // 1 USD = 10 SEK
//                case OfficeLocation.USA:
//                    return "USD"; // 1 USD = 1 USD
//                case OfficeLocation.Turkey:
//                    return "TRY";
//                default:
//                    return "USD"; // Default to 1:1 if office is unknown
//            }
//        }
//    }
//}
