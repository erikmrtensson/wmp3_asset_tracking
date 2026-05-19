Console.WriteLine("Hello, World!");


List<Asset> assets = new List<Asset>();

assets.Add(new Computer("Apple", "MacBook Pro", new DateTime(2024, 3, 15), 1500, 15000, "Sweden"));
assets.Add(new Computer("Lenovo", "ThinkPad X1", new DateTime(2023, 10, 1), 1200, 12000, "Sweden"));
assets.Add(new MobilePhone("Samsung", "Galaxy S23", new DateTime(2025, 1, 10), 800, 8000, "USA"));
assets.Add(new MobilePhone("Apple", "iPhone 15", new DateTime(2024, 12, 5), 999, 9990, "USA"));
assets.Add(new Computer("Apple", "MacBook Pro", new DateTime(2022, 8, 10), 1500, 15000, "Sweden"));
assets.Add(new Computer("Asus", "ZenBook", new DateTime(2023, 1, 5), 1100, 11000, "Sweden"));
assets.Add(new Computer("Lenovo", "ThinkPad X1", new DateTime(2024, 4, 1), 1200, 12000, "Sweden"));
assets.Add(new MobilePhone("Nokia", "XR20", new DateTime(2022, 9, 1), 500, 5000, "USA"));
assets.Add(new MobilePhone("Samsung", "Galaxy S23", new DateTime(2025, 1, 10), 800, 8000, "USA"));

// SORT LIST FIRST BY TYPE THEN BY PURCHASE DATE
List<Asset> sortedList = assets.OrderBy(a => a.GetAssetType()).ThenBy(a => a.PurchaseDate).ToList();


Console.WriteLine("ASSET LIST");
Console.WriteLine("-------------------------------------------------------------------------------------");
Console.WriteLine($"{"Type",-15}{"Brand",-15}{"Model",-16}{"Purchase Date"}\t{"Status"}");
Console.WriteLine("-------------------------------------------------------------------------------------");

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

    Console.ForegroundColor = color;
    Console.WriteLine($"{asset.GetAssetType(),-15}{asset.Brand,-15}{asset.Model,-16}{asset.PurchaseDate:yyyy-MM-dd}\t{status}");
    Console.ResetColor();
}

abstract class Asset
{
    protected Asset(string brand, string model, DateTime purchaseDate, decimal priceUSD, decimal priceLocal, string office)
    {
        Brand = brand;
        Model = model;
        PurchaseDate = purchaseDate;
        PriceUSD = priceUSD;
        PriceLocal = priceLocal;
        Office = office;
    }

    public string Brand { get; set; }
    public string Model { get; set; }
    public DateTime PurchaseDate { get; set; }
    public decimal PriceUSD { get; set; }
    public decimal PriceLocal { get; set; }
    public string Office { get; set; }

    public abstract string GetAssetType();


}

class Computer : Asset
{
    public Computer(string brand, string model, DateTime purchaseDate, decimal priceUSD, decimal priceLocal, string office)
        : base(brand, model, purchaseDate, priceUSD, priceLocal, office)
    {
    }
    public override string GetAssetType()
    {
        return "Computer";
    }
}

class MobilePhone : Asset
{
    public MobilePhone(string brand, string model, DateTime purchaseDate, decimal priceUSD, decimal priceLocal, string office)
        : base(brand, model, purchaseDate, priceUSD, priceLocal, office)
    {
    }
    public override string GetAssetType()
    {
        return "Mobile Phone";
    }

}