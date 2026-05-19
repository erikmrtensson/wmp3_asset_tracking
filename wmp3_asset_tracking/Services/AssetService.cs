using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using wmp3_asset_tracking.Models;

namespace wmp3_asset_tracking.Services
{
    internal class AssetService
    {
        private List<Asset> _assets;

        public AssetService(List<Asset> assets)
        {
            _assets = assets;
        }

        public void ShowAssets(List<Asset> assets)
        {
            if (assets.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No assets available.\n");
                Console.ResetColor();
                return;
            }

            List<Asset> sortedList = assets.OrderBy(a => a.Office).ThenBy(a => a.PurchaseDate).ToList();

            Console.WriteLine("ASSET LIST");
            Console.WriteLine(new string('-', 110));
            Console.WriteLine($"{"ID",-6}{"Office",-12}{"Type",-15}{"Brand",-12}{"Model",-16}{"Price",-14}{"Age",-16}{"Purchase Date",-16}{"Status"}");
            Console.WriteLine(new string('-', 110));

            foreach (var asset in sortedList)
            {
                DateTime expiryDate = asset.PurchaseDate.AddYears(3);
                TimeSpan timeLeft = expiryDate - DateTime.Today;

                string status = "";
                ConsoleColor color = ConsoleColor.White;

                if (timeLeft.Days < 90)
                {
                    color = ConsoleColor.Red;
                    status = "RED";
                }
                else if (timeLeft.Days < 180)
                {
                    color = ConsoleColor.Yellow;
                    status = "YELLOW";
                }

                decimal localPrice = asset.PriceUSD * CurrencyService.GetExchangeRate(asset.Office);
                string currency = CurrencyService.GetCurrency(asset.Office);
                string priceDisplay = $"{localPrice:F0} {currency}";

                string purchaseDate = asset.PurchaseDate.ToString("yyyy-MM-dd");

                Console.ForegroundColor = color;
                Console.WriteLine($"{asset.Id,-6}{asset.Office,-12}{asset.GetAssetType(),-15}{asset.Brand,-12}{asset.Model,-16}{priceDisplay,-14}{asset.GetAge(),-16}{purchaseDate,-16}{status}");
                Console.ResetColor();
            }
            Console.WriteLine();
        }

        public void AddAsset(List<Asset> assets)
        {
            Console.WriteLine("Asset type: (1) Computer  (2) Mobile Phone");
            Console.Write("Select: ");
            string type = Console.ReadLine()?.Trim() ?? "";

            if (type != "1" && type != "2")
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid type.\n");
                Console.ResetColor();
                return;
            }

            Console.Write("Brand: ");
            string brand = Console.ReadLine()?.Trim() ?? "";

            if (string.IsNullOrWhiteSpace(brand))
            {
                PrintError("Brand cannot be empty or just spaces.");
                return;
            }

            Console.Write("Model: ");
            string model = Console.ReadLine()?.Trim() ?? "";

            if (string.IsNullOrWhiteSpace(model))
            {
                PrintError("Model cannot be empty or just spaces.");
                return;
            }

            Console.Write("Purchase Date (yyyy-MM-dd): ");

            if (!DateTime.TryParse(Console.ReadLine(), out DateTime purchaseDate))
            {
                PrintError("Invalid date.");
                return;
            }

            Console.Write("Price (USD): ");

            if (!decimal.TryParse(Console.ReadLine(), out decimal price) || price < 0)
            {
                PrintError("Invalid price.");
                return;
            }

            Console.Write("Office (Sweden/USA/Turkey): ");
            string office = Console.ReadLine()?.Trim().ToLower() ?? "";

            if (office != "sweden" && office != "usa" && office != "turkey")
            {
                PrintError("Invalid office.");
                return;
            }

            // Capitalize letters for proper display
            if (office == "usa")
            {
                office = "USA";
            }
            else
            {
                office = char.ToUpper(office[0]) + office.Substring(1);
            }

            // Create asset based on type
            if (type == "1")
            {
                assets.Add(new Computer(brand, model, purchaseDate, price, office));
            }
            else
            {
                assets.Add(new MobilePhone(brand, model, purchaseDate, price, office));
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Asset added successfully!\n");
            Console.ResetColor();
        }

        public void SearchAssets(List<Asset> assets)
        {
            Console.Write("Search by brand or model: ");
            string query = Console.ReadLine()?.Trim().ToLower() ?? "";

            List<Asset> results = assets
                .Where(a => a.Brand.ToLower().Contains(query) || a.Model.ToLower().Contains(query))
                .ToList();

            if (results.Count == 0)
            {
                PrintError("No assets found.");
                return;
            }

            ShowAssets(results);
        }

        public void RemoveAsset(List<Asset> assets)
        {
            ShowAssets(assets);

            Console.Write("Enter Asset ID to remove: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            { 
                PrintError("Invalid ID."); 
                return; 
            }

            Asset? asset = assets.FirstOrDefault(a => a.Id == id);
            if (asset == null)
            { 
                PrintError("Asset not found."); 
                return; 
            }

            assets.Remove(asset);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Asset '{asset.Brand} {asset.Model}' removed.\n");
            Console.ResetColor();
        }

        public void PrintError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"ERROR: {message}\n");
            Console.ResetColor();
        }

    }
}
