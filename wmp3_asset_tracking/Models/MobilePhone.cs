using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace wmp3_asset_tracking.Models
{
    public class MobilePhone : Asset
    {
        protected MobilePhone() { } // EF Core

        public MobilePhone(string brand, string model, DateTime purchaseDate, decimal priceUSD, OfficeLocation office, string serialNumber)
            : base(brand, model, purchaseDate, priceUSD, office, serialNumber) { }

        public override string GetAssetType() => "Mobile Phone";
    }
}
