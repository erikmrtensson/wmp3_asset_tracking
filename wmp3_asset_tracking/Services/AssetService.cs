using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;
using wmp3_asset_tracking.Models;

namespace wmp3_asset_tracking.Services
{
    internal class AssetService
    {
        private List<Asset> _assets = new List<Asset>();

        private static readonly string FilePath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Data", "assets.json"); // Path.Combine is used to ensure cross-platform compatibility placing it in a Data folder three levels up from the executable

        public void ShowAssets(List<Asset>? assetsToShow = null)
        {
            List<Asset> targetList = assetsToShow ?? _assets;

            if (targetList.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No assets available.\n");
                Console.ResetColor();
                return;
            }

            List<Asset> sortedList = targetList.OrderBy(a => a.Office).ThenBy(a => a.PurchaseDate).ToList();

            Console.WriteLine("ASSET LIST");
            Console.WriteLine(new string('-', 110));
            Console.WriteLine($"{"ID",-6}{"Office",-12}{"Type",-15}{"Brand",-12}{"Model",-16}{"Price",-14}{"Age",-16}{"Purchase Date",-16}{"Status"}");
            Console.WriteLine(new string('-', 110));

            foreach (var _asset in sortedList)
            {
                DateTime expiryDate = _asset.PurchaseDate.AddYears(3);
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

                // decimal localPrice = _asset.PriceUSD * CurrencyService.GetExchangeRate(_asset.Office);

                decimal localPrice = CurrencyService.GetConvertedPrice(_asset.PriceUSD, _asset.Office);

                string currency = CurrencyService.GetCurrency(_asset.Office);
                string priceDisplay = $"{localPrice:F0} {currency}";

                string purchaseDate = _asset.PurchaseDate.ToString("yyyy-MM-dd");

                Console.ForegroundColor = color;
                Console.WriteLine($"{_asset.Id,-6}{_asset.Office,-12}{_asset.GetAssetType(),-15}{_asset.Brand,-12}{_asset.Model,-16}{priceDisplay,-14}{_asset.GetAge(),-16}{purchaseDate,-16}{status}");
                Console.ResetColor();
            }
            Console.WriteLine();
        }

        public void AddAsset()
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

            // Clearer Enum Selection Menu
            Console.WriteLine("Office: (1) Sweden  (2) USA  (3) Turkey");
            Console.Write("Select: ");
            string officeChoice = Console.ReadLine()?.Trim() ?? "";

            OfficeLocation office;

            if (officeChoice == "1")
            {
                office = OfficeLocation.Sweden;
            }
            else if (officeChoice == "2")
            {
                office = OfficeLocation.USA;
            }
            else if (officeChoice == "3")
            {
                office = OfficeLocation.Turkey;
            }
            else
            {
                PrintError("Invalid office selection.");
                return;
            }

            
            if (type == "1")
            {
                _assets.Add(new Computer(brand, model, purchaseDate, price, office));
            }
            else
            {
                _assets.Add(new MobilePhone(brand, model, purchaseDate, price, office));
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Asset added successfully!");
            Console.WriteLine("Saving changes to assets.json\n");
            SaveAssets();
            Console.ResetColor();
        }

        public void SearchAssets()
        {
            Console.Write("Search by brand or model: ");
            string query = Console.ReadLine()?.Trim().ToLower() ?? "";

            if (query == "")
            {
                PrintError("Search query cannot be empty.");
                return;
            }

            List<Asset> results = _assets
                .Where(a => a.Brand.ToLower().Contains(query) || a.Model.ToLower().Contains(query))
                .ToList();

            if (results.Count == 0)
            {
                PrintError("No assets found.");
                return;
            }

            ShowAssets(results);
        }

        public void RemoveAsset()
        {
            ShowAssets();

            Console.Write("Enter Asset ID to remove: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            { 
                PrintError("Invalid ID."); 
                return; 
            }

            Asset? asset = _assets.FirstOrDefault(a => a.Id == id);
            if (asset == null)
            { 
                PrintError("Asset not found."); 
                return; 
            }

            _assets.Remove(asset);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Asset '{asset.Brand} {asset.Model}' removed.");
            Console.WriteLine("Saving changes to asset.json\n");
            SaveAssets();
            Console.ResetColor();
        }

        public void PrintError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"ERROR: {message}\n");
            Console.ResetColor();
        }


        // SAVE FILE
        public void SaveAssets()
        {
            string? directory = Path.GetDirectoryName(FilePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            JsonSerializerOptions options = new JsonSerializerOptions();
            options.WriteIndented = true;
            string json = JsonSerializer.Serialize(_assets, options);

            File.WriteAllText(FilePath, json);
            //Console.ForegroundColor = ConsoleColor.Green;
            //Console.WriteLine($"Assets saved to {FilePath}\n");
            //Console.ResetColor();
        }

        public void LoadAssets()
        {
            Console.WriteLine("Loading assets from file...");

            if (!File.Exists(FilePath))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("No save file found. Starting with an empty list.\n");
                Console.ResetColor();
                return;
            }

            try
            {
                string json = File.ReadAllText(FilePath);
                // Deserializes polymorphically directly into Computer/MobilePhone subclasses!
                List<Asset>? loadedAssets = JsonSerializer.Deserialize<List<Asset>>(json);

                if (loadedAssets != null)
                {
                    _assets = loadedAssets;

                    if (_assets.Count > 0)
                    {
                        Asset.SetNextId(_assets.Max(a => a.Id) + 1);
                    }
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{_assets.Count} assets loaded successfully.\n");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                PrintError($"Failed to parse file: {ex.Message}");
            }
        }

    }
}
