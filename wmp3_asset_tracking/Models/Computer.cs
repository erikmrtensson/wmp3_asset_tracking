using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace wmp3_asset_tracking.Models
{
    public class Computer : Asset
    {
        protected Computer() { } // EF Core

        public Computer(string brand, string model, DateTime purchaseDate, decimal priceUSD, OfficeLocation office, string serialNumber)
            : base(brand, model, purchaseDate, priceUSD, office, serialNumber) { }

        public override string GetAssetType() => "Computer";
    }
}
