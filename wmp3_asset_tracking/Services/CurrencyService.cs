using System;
using System.Collections.Generic;
using System.Text;
using wmp3_asset_tracking.Models;

namespace wmp3_asset_tracking.Services
{
    internal class CurrencyService
    {
        public static decimal GetExchangeRate(OfficeLocation office)
        {
            switch (office)
            {
                case OfficeLocation.Sweden:
                    return 10.0m; // 1 USD = 10 SEK
                case OfficeLocation.USA:
                    return 1.0m; // 1 USD = 1 USD
                case OfficeLocation.Turkey:
                    return 32.0m;
                default:
                    return 1.0m; // Default to 1:1 if office is unknown
            }
        }

        public static string GetCurrency(OfficeLocation office)
        {
            switch (office)
            {
                case OfficeLocation.Sweden:
                    return "SEK"; // 1 USD = 10 SEK
                case OfficeLocation.USA:
                    return "USD"; // 1 USD = 1 USD
                case OfficeLocation.Turkey:
                    return "TRY";
                default:
                    return "USD"; // Default to 1:1 if office is unknown
            }
        }
    }
}
