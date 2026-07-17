using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace wmp3_asset_tracking.Models
{
    public enum OfficeLocation
    {
        Sweden,
        USA,
        Turkey
    }

    public abstract class Asset
    {
        protected Asset() { }

        protected Asset(string brand, string model, DateTime purchaseDate, decimal priceUSD, OfficeLocation office, string serialNumber)
        {
            Brand = brand;
            Model = model;
            PurchaseDate = purchaseDate;
            PriceUSD = priceUSD;
            Office = office;
            SerialNumber = serialNumber;
        }

        public int Id { get; set; }
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public DateTime PurchaseDate { get; set; }
        public decimal PriceUSD { get; set; }
        public OfficeLocation Office { get; set; }
        public string SerialNumber { get; set; } = string.Empty;

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
                days += DateTime.DaysInMonth(today.Year, today.Month == 1 ? 12 : today.Month - 1);
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
