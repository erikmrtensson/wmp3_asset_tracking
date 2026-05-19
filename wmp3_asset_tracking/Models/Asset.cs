using System;
using System.Collections.Generic;
using System.Text;

namespace wmp3_asset_tracking.Models
{
    internal abstract class Asset
    {
        private static int _nextId = 1001;
        protected Asset(string brand, string model, DateTime purchaseDate, decimal priceUSD, string office)
        {
            Id = _nextId++;
            Brand = brand;
            Model = model;
            PurchaseDate = purchaseDate;
            PriceUSD = priceUSD;
            Office = office;
        }

        public int Id { get; private set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal PriceUSD { get; set; }
        public string Office { get; set; }

        public abstract string GetAssetType();

        public string GetAge()
        {
            DateTime today = DateTime.Today;
            int years = today.Year - PurchaseDate.Year;
            int months = today.Month - PurchaseDate.Month;
            int days = today.Day - PurchaseDate.Day;

            if (days < 0)
            {
                months--;
                days += DateTime.DaysInMonth(today.Year, today.Month - 1);
            }

            if (months < 0)
            {
                years--;
                months += 12;
            }

            return $"{years}y {months}m {days}d";
        }

    }
}
