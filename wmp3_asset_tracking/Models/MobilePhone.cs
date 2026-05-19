using System;
using System.Collections.Generic;
using System.Text;

namespace wmp3_asset_tracking.Models
{
    internal class MobilePhone : Asset
    {
        public MobilePhone(string brand, string model, DateTime purchaseDate, decimal priceUSD, string office)
    : base(brand, model, purchaseDate, priceUSD, office)
        {
        }
        public override string GetAssetType()
        {
            return "Mobile Phone";
        }

    }
}
