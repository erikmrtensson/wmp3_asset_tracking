using System;
using System.Collections.Generic;
using System.Text;

namespace wmp3_asset_tracking.Models
{
    internal class CurrencyObj // This class represents a currency and its exchange rate from EUR. It is used to store the exchange rates fetched from the ECB.
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
