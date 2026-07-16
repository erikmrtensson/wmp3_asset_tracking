using wmp3_asset_tracking.Data;
using wmp3_asset_tracking.Models;
using System.Text.Json;

namespace wmp3_asset_tracking.Services
{
    public class AssetService
    {
        private readonly AssetContext _context;

        private static readonly string ExportDirectory = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Exports");

        public AssetService(AssetContext context)
        {
            _context = context;
        }

        public void SeedDatabase()
        {
            if (_context.Assets.Any())
            {
                return; 
            }

            var seedAssets = new List<Asset>
            {
                // Sweden — mix of statuses
                new Computer("Lenovo", "ThinkPad X1", DateTime.Today.AddYears(-3).AddMonths(1), 1600m, OfficeLocation.Sweden), // RED
                new MobilePhone("Apple", "iPhone 15", DateTime.Today.AddMonths(-6), 999m, OfficeLocation.Sweden), // NORMAL

                // USA
                new Computer("Dell", "XPS 15", DateTime.Today.AddYears(-2).AddMonths(-9), 1800m, OfficeLocation.USA), // YELLOW
                new MobilePhone("Samsung", "Galaxy S24", DateTime.Today.AddYears(-1), 899m, OfficeLocation.USA), // NORMAL

                // Turkey
                new Computer("Asus", "ZenBook 14", DateTime.Today.AddYears(-2).AddMonths(-6), 1100m, OfficeLocation.Turkey), // NORMAL
                new MobilePhone("Nokia", "XR20", DateTime.Today.AddYears(-3).AddMonths(2), 450m, OfficeLocation.Turkey), // RED

                // Additional Sweden
                new Computer("HP", "EliteBook 840", DateTime.Today.AddYears(-1).AddMonths(-2), 1400m, OfficeLocation.Sweden), // NORMAL
                new MobilePhone("Samsung", "Galaxy A54", DateTime.Today.AddYears(-2).AddMonths(-8), 350m, OfficeLocation.Sweden), // YELLOW
                new Computer("Lenovo", "Legion 5", DateTime.Today.AddYears(-3).AddMonths(3), 1500m, OfficeLocation.Sweden), // RED

                // Additional USA
                new MobilePhone("Apple", "iPhone 14", DateTime.Today.AddYears(-2).AddMonths(-10), 899m, OfficeLocation.USA), // YELLOW
                new Computer("Apple", "MacBook Pro 14", DateTime.Today.AddMonths(-3), 2400m, OfficeLocation.USA), // NORMAL
                new MobilePhone("Google", "Pixel 8", DateTime.Today.AddYears(-3).AddMonths(1), 699m, OfficeLocation.USA), // RED
                new Computer("Dell", "Latitude 5420", DateTime.Today.AddYears(-3).AddMonths(4), 1200m, OfficeLocation.USA), // RED (expired)

                // Additional Turkey
                new MobilePhone("Samsung", "Galaxy S23", DateTime.Today.AddYears(-1).AddMonths(-5), 750m, OfficeLocation.Turkey), // NORMAL
                new Computer("Asus", "ROG Strix", DateTime.Today.AddYears(-2).AddMonths(-7), 1900m, OfficeLocation.Turkey), // YELLOW
                new MobilePhone("Nokia", "G42", DateTime.Today.AddYears(-1).AddMonths(-1), 250m, OfficeLocation.Turkey), // NORMAL
                new Computer("HP", "Pavilion 15", DateTime.Today.AddYears(-3).AddMonths(6), 950m, OfficeLocation.Turkey), // RED (expired)
            };

            _context.Assets.AddRange(seedAssets);
            _context.SaveChanges();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Seeded database with {seedAssets.Count} sample assets.\n");
            Console.ResetColor();
        }


        public void ShowAssets(List<Asset>? assetsToShow = null)
        {
            List<Asset> rawList = assetsToShow ?? _context.Assets.ToList();

            if (rawList.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No assets available.\n");
                Console.ResetColor();
                return;
            }

            List<Asset> targetList = rawList
                .OrderBy(a => a.GetAssetType())
                .ThenBy(a => a.PurchaseDate)
                .ToList();

            Console.WriteLine("ASSET LIST");
            Console.WriteLine(new string('-', 120));
            Console.WriteLine($"{"ID",-6}{"Type",-15}{"Brand",-12}{"Model",-16}{"Office",-10}{"Purchase Date",-16}{"Price (USD)",-14}{"Currency",-10}{"Local Price",-14}{"Status"}");
            Console.WriteLine(new string('-', 120));

            foreach (var asset in targetList)
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

                decimal localPrice = CurrencyService.GetConvertedPrice(asset.PriceUSD, asset.Office);
                string currency = CurrencyService.GetCurrency(asset.Office);

                string usdDisplay = $"${asset.PriceUSD:F2}";
                string localDisplay = $"{localPrice:F0}";
                string purchaseDate = asset.PurchaseDate.ToString("yyyy-MM-dd");

                Console.ForegroundColor = color;
                Console.WriteLine($"{asset.Id,-6}{asset.GetAssetType(),-15}{asset.Brand,-12}{asset.Model,-16}{asset.Office,-10}{purchaseDate,-16}{usdDisplay,-14}{currency,-10}{localDisplay,-14}{status}");
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
                PrintError("Invalid type.");
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

            Console.WriteLine("Office: (1) Sweden  (2) USA  (3) Turkey");
            Console.Write("Select: ");
            string officeChoice = Console.ReadLine()?.Trim() ?? "";

            OfficeLocation office;
            switch (officeChoice)
            {
                case "1": office = OfficeLocation.Sweden; break;
                case "2": office = OfficeLocation.USA; break;
                case "3": office = OfficeLocation.Turkey; break;
                default:
                    PrintError("Invalid office selection.");
                    return;
            }

            Asset newAsset = type == "1"
                ? new Computer(brand, model, purchaseDate, price, office)
                : new MobilePhone(brand, model, purchaseDate, price, office);

            _context.Assets.Add(newAsset);
            _context.SaveChanges(); // replaces SaveAssets() / JSON write

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Asset added successfully!\n");
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

            List<Asset> results = _context.Assets
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

            Asset? asset = _context.Assets.FirstOrDefault(a => a.Id == id);
            if (asset == null)
            {
                PrintError("Asset not found.");
                return;
            }

            _context.Assets.Remove(asset);
            _context.SaveChanges();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Asset '{asset.Brand} {asset.Model}' removed.\n");
            Console.ResetColor();
        }

        public void UpdateAsset()
        {
            ShowAssets();

            Console.Write("Enter Asset ID to update: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                PrintError("Invalid ID.");
                return;
            }

            Asset? asset = _context.Assets.FirstOrDefault(a => a.Id == id);
            if (asset == null)
            {
                PrintError("Asset not found.");
                return;
            }

            Console.WriteLine($"Updating '{asset.Brand} {asset.Model}'. Press Enter to keep the current value.\n");

            Console.Write($"Brand [{asset.Brand}]: ");
            string brandInput = Console.ReadLine()?.Trim() ?? "";
            if (!string.IsNullOrWhiteSpace(brandInput))
                asset.Brand = brandInput;

            Console.Write($"Model [{asset.Model}]: ");
            string modelInput = Console.ReadLine()?.Trim() ?? "";
            if (!string.IsNullOrWhiteSpace(modelInput))
                asset.Model = modelInput;

            Console.Write($"Purchase Date [{asset.PurchaseDate:yyyy-MM-dd}]: ");
            string dateInput = Console.ReadLine()?.Trim() ?? "";
            if (!string.IsNullOrWhiteSpace(dateInput))
            {
                if (!DateTime.TryParse(dateInput, out DateTime newDate))
                {
                    PrintError("Invalid date. Update cancelled.");
                    return;
                }
                asset.PurchaseDate = newDate;
            }

            Console.Write($"Price (USD) [{asset.PriceUSD}]: ");
            string priceInput = Console.ReadLine()?.Trim() ?? "";
            if (!string.IsNullOrWhiteSpace(priceInput))
            {
                if (!decimal.TryParse(priceInput, out decimal newPrice) || newPrice < 0)
                {
                    PrintError("Invalid price. Update cancelled.");
                    return;
                }
                asset.PriceUSD = newPrice;
            }

            Console.Write($"Office (1=Sweden, 2=USA, 3=Turkey) [{asset.Office}]: ");
            string officeInput = Console.ReadLine()?.Trim() ?? "";
            if (!string.IsNullOrWhiteSpace(officeInput))
            {
                switch (officeInput)
                {
                    case "1": asset.Office = OfficeLocation.Sweden; break;
                    case "2": asset.Office = OfficeLocation.USA; break;
                    case "3": asset.Office = OfficeLocation.Turkey; break;
                    default:
                        PrintError("Invalid office selection. Update cancelled.");
                        return;
                }
            }

            _context.SaveChanges();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Asset '{asset.Brand} {asset.Model}' updated.\n");
            Console.ResetColor();
        }

        // REPORTING

        private string BuildReportText()
        {
            List<Asset> assets = _context.Assets.ToList();
            var sb = new System.Text.StringBuilder();

            if (assets.Count == 0)
            {
                sb.AppendLine("No assets available for reporting.");
                return sb.ToString();
            }

            var officeGroups = assets.GroupBy(a => a.Office).OrderBy(g => g.Key).ToList();

            sb.AppendLine("\n================ REPORT ================\n");

            sb.AppendLine("Office Asset Counts");
            sb.AppendLine(new string('-', 42));
            foreach (var group in officeGroups)
            {
                string label = $"{group.Key} Office";
                sb.AppendLine($"{label,-15}: {group.Count()}");
            }

            sb.AppendLine("\nTotal Value Per Office");
            sb.AppendLine(new string('-', 42));
            foreach (var group in officeGroups)
            {
                decimal totalLocal = group.Sum(a => CurrencyService.GetConvertedPrice(a.PriceUSD, a.Office));
                string currency = CurrencyService.GetCurrency(group.Key);
                string label = $"{group.Key} Office";
                sb.AppendLine($"{label,-15}: {totalLocal:N0} {currency}");
            }

            sb.AppendLine("\nAssets Near Expiration");
            sb.AppendLine(new string('-', 42));
            var expiring = assets
                .Where(a => (a.PurchaseDate.AddYears(3) - DateTime.Today).Days < 180)
                .OrderBy(a => a.PurchaseDate.AddYears(3))
                .ToList();

            if (expiring.Count == 0)
                sb.AppendLine("None.");
            else
                foreach (var a in expiring)
                    sb.AppendLine($"{a.Brand} {a.Model}");

            sb.AppendLine("\nTop 5 Most Expensive Assets (USD)");
            sb.AppendLine(new string('-', 42));
            foreach (var a in assets.OrderByDescending(a => a.PriceUSD).Take(5))
                sb.AppendLine($"{a.Brand} {a.Model}: ${a.PriceUSD:N2}");

            sb.AppendLine("\n==========================================\n");

            return sb.ToString();
        }

        public void ShowReport()
        {
            Console.Write(BuildReportText());
        }

        public void ExportReport()
        {
            string reportText = BuildReportText();

            Directory.CreateDirectory(ExportDirectory);
            string path = Path.Combine(ExportDirectory, "report.txt");
            File.WriteAllText(path, reportText);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Report exported to {path}\n");
            Console.ResetColor();
        }

        // SEARCH

        public void SearchByOffice()
        {
            Console.WriteLine("Office: (1) Sweden  (2) USA  (3) Turkey");
            Console.Write("Select: ");
            string choice = Console.ReadLine()?.Trim() ?? "";

            OfficeLocation office;
            switch (choice)
            {
                case "1": office = OfficeLocation.Sweden; break;
                case "2": office = OfficeLocation.USA; break;
                case "3": office = OfficeLocation.Turkey; break;
                default:
                    PrintError("Invalid office selection.");
                    return;
            }

            List<Asset> results = _context.Assets.Where(a => a.Office == office).ToList();

            if (results.Count == 0)
            {
                PrintError("No assets found for that office.");
                return;
            }

            ShowAssets(results);
        }

        public void SearchByYear()
        {
            Console.Write("Enter purchase year (yyyy): ");
            if (!int.TryParse(Console.ReadLine(), out int year) || year < 1990 || year > 2100)
            {
                PrintError("Invalid year.");
                return;
            }

            List<Asset> results = _context.Assets
                .Where(a => a.PurchaseDate.Year == year)
                .ToList();

            if (results.Count == 0)
            {
                PrintError("No assets found for that year.");
                return;
            }

            ShowAssets(results);
        }

        // FILTERING

        public void FilterExpired()
        {
            // AddYears() isn't SQL-translatable, so pull to memory first
            List<Asset> results = _context.Assets
                .ToList()
                .Where(a => a.PurchaseDate.AddYears(3) < DateTime.Today)
                .ToList();

            if (results.Count == 0)
            {
                PrintError("No expired assets.");
                return;
            }

            ShowAssets(results);
        }

        public void FilterComputers()
        {
            // OfType<T> against a TPH discriminator column DOES translate to SQL
            List<Asset> results = _context.Assets.OfType<Computer>().Cast<Asset>().ToList();

            if (results.Count == 0)
            {
                PrintError("No computers found.");
                return;
            }

            ShowAssets(results);
        }

        public void FilterMobile()
        {
            List<Asset> results = _context.Assets.OfType<MobilePhone>().Cast<Asset>().ToList();

            if (results.Count == 0)
            {
                PrintError("No mobile phones found.");
                return;
            }

            ShowAssets(results);
        }

        // EXPORT

        public void ExportToTxt()
        {
            List<Asset> assets = _context.Assets
                .ToList()
                .OrderBy(a => a.GetAssetType())
                .ThenBy(a => a.PurchaseDate)
                .ToList();

            if (assets.Count == 0) { PrintError("No assets to export."); return; }

            Directory.CreateDirectory(ExportDirectory);
            string path = Path.Combine(ExportDirectory, "assets.txt");

            using var writer = new StreamWriter(path);

            writer.WriteLine("ASSET LIST");
            writer.WriteLine(new string('-', 140));
            writer.WriteLine($"{"ID",-6}{"Brand",-12}{"Model",-16}{"Office",-10}{"Purchase Date",-16}{"Price (USD)",-14}{"Currency",-10}{"Local Price",-14}{"Type",-15}{"Status"}");
            writer.WriteLine(new string('-', 140));

            foreach (var asset in assets)
            {
                DateTime expiryDate = asset.PurchaseDate.AddYears(3);
                TimeSpan timeLeft = expiryDate - DateTime.Today;

                string status = "NORMAL";
                if (timeLeft.Days < 90) status = "RED";
                else if (timeLeft.Days < 180) status = "YELLOW";

                decimal localPrice = CurrencyService.GetConvertedPrice(asset.PriceUSD, asset.Office);
                string currency = CurrencyService.GetCurrency(asset.Office);

                string usdDisplay = $"${asset.PriceUSD:F2}";
                string localDisplay = $"{localPrice:F0}";
                string purchaseDate = asset.PurchaseDate.ToString("yyyy-MM-dd");

                writer.WriteLine($"{asset.Id,-6}{asset.Brand,-12}{asset.Model,-16}{asset.Office,-10}{purchaseDate,-16}{usdDisplay,-14}{currency,-10}{localDisplay,-14}{asset.GetAssetType(),-15}{status}");
            }

            writer.WriteLine(new string('-', 140));

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Exported {assets.Count} assets to {path}\n");
            Console.ResetColor();
        }

        public void ExportToCsv()
        {
            List<Asset> assets = _context.Assets.ToList();
            if (assets.Count == 0) { PrintError("No assets to export."); return; }

            Directory.CreateDirectory(ExportDirectory);
            string path = Path.Combine(ExportDirectory, "assets.csv");

            using var writer = new StreamWriter(path);
            writer.WriteLine("Id,Type,Brand,Model,Office,PurchaseDate,PriceUSD");
            foreach (var a in assets)
                writer.WriteLine($"{a.Id},{a.GetAssetType()},{a.Brand},{a.Model},{a.Office},{a.PurchaseDate:yyyy-MM-dd},{a.PriceUSD}");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Exported {assets.Count} assets to {path}\n");
            Console.ResetColor();
        }

        public void ExportToJson()
        {
            List<Asset> assets = _context.Assets.ToList();
            if (assets.Count == 0) { PrintError("No assets to export."); return; }

            Directory.CreateDirectory(ExportDirectory);
            string path = Path.Combine(ExportDirectory, "assets.json");

            var exportData = assets.Select(a => new
            {
                a.Id,
                Type = a.GetAssetType(),
                a.Brand,
                a.Model,
                Office = a.Office.ToString(),
                PurchaseDate = a.PurchaseDate.ToString("yyyy-MM-dd"),
                a.PriceUSD
            });

            string json = JsonSerializer.Serialize(exportData, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Exported {assets.Count} assets to {path}\n");
            Console.ResetColor();
        }

        // MENUS

        public void SearchFilterMenu()
        {
            Console.WriteLine("1. Search by Brand/Model");
            Console.WriteLine("2. Search by Office");
            Console.WriteLine("3. Search by Purchase Year");
            Console.WriteLine("4. Filter: Expired Only");
            Console.WriteLine("5. Filter: Computers Only");
            Console.WriteLine("6. Filter: Mobile Phones Only");
            Console.Write("Select: ");
            string choice = Console.ReadLine()?.Trim() ?? "";
            Console.WriteLine();

            switch (choice)
            {
                case "1": SearchAssets(); break;
                case "2": SearchByOffice(); break;
                case "3": SearchByYear(); break;
                case "4": FilterExpired(); break;
                case "5": FilterComputers(); break;
                case "6": FilterMobile(); break;
                default: PrintError("Invalid choice."); break;
            }
        }

        public void ExportMenu()
        {
            Console.WriteLine("1. Export Asset List to TXT");
            Console.WriteLine("2. Export Asset List to CSV");
            Console.WriteLine("3. Export Asset List to JSON");
            Console.WriteLine("4. Export Report to TXT");
            Console.Write("Select: ");
            string choice = Console.ReadLine()?.Trim() ?? "";
            Console.WriteLine();

            switch (choice)
            {
                case "1": ExportToTxt(); break;
                case "2": ExportToCsv(); break;
                case "3": ExportToJson(); break;
                case "4": ExportReport(); break;
                default: PrintError("Invalid choice."); break;
            }
        }

        public void PrintError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"ERROR: {message}\n");
            Console.ResetColor();
        }
    }
}