using System;
using System.Collections.Generic;
using System.Text;

namespace wmp3_asset_tracking.Models
{
    internal class Computer : Asset
    {
        public Computer(string brand, string model, DateTime purchaseDate, decimal priceUSD, OfficeLocation office)
    : base(brand, model, purchaseDate, priceUSD, office)
        {
        }
        public override string GetAssetType()
        {
            return "Computer";
        }
    }
}
