using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace wmp3_asset_tracking.Models
{
    internal class MobilePhone : Asset
    {
        public MobilePhone(string brand, string model, DateTime purchaseDate, decimal priceUSD, OfficeLocation office)
    : base(brand, model, purchaseDate, priceUSD, office)
        {
        }

        [JsonConstructor]
        public MobilePhone(int id, string brand, string model, DateTime purchaseDate, decimal priceUSD, OfficeLocation office)
    : base(id, brand, model, purchaseDate, priceUSD, office)
        {
        }
        public override string GetAssetType()
        {
            return "Mobile Phone";
        }

    }
}
