using System;
using System.Collections.Generic;
using System.Text;

namespace wmp3_asset_tracking.Services
{
    internal class CurrencyService
    {
        public static decimal GetExchangeRate(string office)
        {
            switch (office)
            {
                case "Sweden":
                    return 10.0m; // 1 USD = 10 SEK
                case "USA":
                    return 1.0m; // 1 USD = 1 USD
                case "Turkey":
                    return 32.0m;
                default:
                    return 1.0m; // Default to 1:1 if office is unknown
            }
        }

        public static string GetCurrency(string office)
        {
            switch (office)
            {
                case "Sweden":
                    return "SEK"; // 1 USD = 10 SEK
                case "USA":
                    return "USD"; // 1 USD = 1 USD
                case "Turkey":
                    return "TRY";
                default:
                    return "USD"; // Default to 1:1 if office is unknown
            }
        }
    }
}
