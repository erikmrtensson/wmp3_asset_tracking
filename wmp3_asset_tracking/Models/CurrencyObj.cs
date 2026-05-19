using System;
using System.Collections.Generic;
using System.Text;

namespace wmp3_asset_tracking.Models
{
    internal class CurrencyObj
    {
        public string CurrencyCode { get; set; }
        public double ExchangeRateFromEUR { get; set; }

        public CurrencyObj(string currencyCode, double exchangeRateFromEUR)
        {
            CurrencyCode = currencyCode;
            ExchangeRateFromEUR = exchangeRateFromEUR;
        }
    }
}
