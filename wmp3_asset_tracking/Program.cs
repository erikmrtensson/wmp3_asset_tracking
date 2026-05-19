Console.WriteLine("Hello, World!");


List<Asset> assets = new List<Asset>();

assets.Add(new Computer("Dell", "XPS 13", new DateTime(2023, 5, 20), 1300, "Sweden"));
assets.Add(new Computer("Apple", "MacBook Pro", new DateTime(2024, 3, 15), 1500, "Sweden"));
assets.Add(new Computer("Lenovo", "ThinkPad X1", new DateTime(2023, 10, 1), 1200, "Sweden"));
assets.Add(new MobilePhone("Samsung", "Galaxy S23", new DateTime(2025, 1, 10), 800, "USA"));
assets.Add(new MobilePhone("Apple", "iPhone 15", new DateTime(2024, 12, 5), 999, "USA"));
assets.Add(new Computer("Apple", "MacBook Pro", new DateTime(2022, 8, 10), 1500, "Sweden"));
assets.Add(new Computer("Asus", "ZenBook", new DateTime(2023, 1, 5), 1100, "Sweden"));
assets.Add(new Computer("Lenovo", "ThinkPad X1", new DateTime(2024, 4, 1), 1200, "Sweden"));
assets.Add(new MobilePhone("Nokia", "XR20", new DateTime(2022, 9, 1), 500, "USA"));
assets.Add(new MobilePhone("Samsung", "Galaxy S23", new DateTime(2025, 1, 10), 800, "USA"));
assets.Add(new Computer("Dell", "XPS 13", new DateTime(2023, 5, 20), 1300, "Turkey"));
assets.Add(new MobilePhone("Samsung", "Galaxy S23", new DateTime(2025, 1, 10), 800, "Turkey"));

// SORT LIST FIRST BY TYPE THEN BY PURCHASE DATE
List<Asset> sortedList = assets.OrderBy(a => a.Office).ThenBy(a => a.PurchaseDate).ToList();


Console.WriteLine("ASSET LIST");
Console.WriteLine("------------------------------------------------------------------------------------------------------------");
Console.WriteLine($"{"Office",-14}{"Type",-15}{"Brand",-15}{"Model",-16}{"Price",-16}{"Purchase Date"}\t{"Status"}");
Console.WriteLine("------------------------------------------------------------------------------------------------------------");

foreach (var asset in sortedList)
{

    // Check Expiry date
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

    Console.ForegroundColor = color;
    Console.WriteLine($"{asset.Office,-14}{asset.GetAssetType(),-15}{asset.Brand,-15}{asset.Model,-16}{priceDisplay,-16}{asset.PurchaseDate:yyyy-MM-dd}\t\t{status}");
    Console.ResetColor();
}

abstract class Asset
{
    protected Asset(string brand, string model, DateTime purchaseDate, decimal priceUSD, string office)
    {
        Brand = brand;
        Model = model;
        PurchaseDate = purchaseDate;
        PriceUSD = priceUSD;
        Office = office;
    }

    public string Brand { get; set; }
    public string Model { get; set; }
    public DateTime PurchaseDate { get; set; }
    public decimal PriceUSD { get; set; }
    public string Office { get; set; }

    public abstract string GetAssetType();


}

class Computer : Asset
{
    public Computer(string brand, string model, DateTime purchaseDate, decimal priceUSD, string office)
        : base(brand, model, purchaseDate, priceUSD, office)
    {
    }
    public override string GetAssetType()
    {
        return "Computer";
    }
}

class MobilePhone : Asset
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

class CurrencyService()
{
    public static decimal GetExchangeRate(string office)
    {
        switch (office)
        {
            case "Sweden":
                return 10.0m; // 1 USD = 10 SEK
            case "USA":
                return 1.0m; // 1 USD = 1 USD
            case "Turkey":
                return 32.0m;
            default:
                return 1.0m; // Default to 1:1 if office is unknown
        }
    }

    public static string GetCurrency(string office)
    {
        switch (office)
        {
            case "Sweden":
                return "SEK"; // 1 USD = 10 SEK
            case "USA":
                return "USD"; // 1 USD = 1 USD
            case "Turkey":
                return "TRY";
            default:
                return "USD"; // Default to 1:1 if office is unknown
        }
    }
}