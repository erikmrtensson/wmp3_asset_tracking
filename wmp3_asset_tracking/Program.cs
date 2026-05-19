using wmp3_asset_tracking.Models;
using wmp3_asset_tracking.Services;

Console.WriteLine("Hello, World!");


List<Asset> assets = new List<Asset>();
AssetService service = new AssetService();

//assets.Add(new Computer("Dell", "XPS 13", new DateTime(2023, 5, 20), 1300, "Sweden"));
//assets.Add(new Computer("Apple", "MacBook Pro", new DateTime(2024, 3, 15), 1500, "Sweden"));
//assets.Add(new Computer("Lenovo", "ThinkPad X1", new DateTime(2023, 10, 1), 1200, "Sweden"));
//assets.Add(new MobilePhone("Samsung", "Galaxy S23", new DateTime(2025, 1, 10), 800, "USA"));
//assets.Add(new MobilePhone("Apple", "iPhone 15", new DateTime(2024, 12, 5), 999, "USA"));
//assets.Add(new Computer("Apple", "MacBook Pro", new DateTime(2022, 8, 10), 1500, "Sweden"));
//assets.Add(new Computer("Asus", "ZenBook", new DateTime(2023, 1, 5), 1100, "Sweden"));
//assets.Add(new Computer("Lenovo", "ThinkPad X1", new DateTime(2024, 4, 1), 1200, "Sweden"));
//assets.Add(new MobilePhone("Nokia", "XR20", new DateTime(2022, 9, 1), 500, "USA"));
//assets.Add(new MobilePhone("Samsung", "Galaxy S23", new DateTime(2025, 1, 10), 800, "USA"));
//assets.Add(new Computer("Dell", "XPS 13", new DateTime(2023, 5, 20), 1300, "Turkey"));
//assets.Add(new MobilePhone("Samsung", "Galaxy S23", new DateTime(2025, 1, 10), 800, "Turkey"));

service.LoadAssets();


while (true)
{
    Console.WriteLine("==================================================");
    Console.WriteLine("         COMPANY ASSET TRACKING SYSTEM           ");
    Console.WriteLine("==================================================");
    Console.WriteLine("1. Add Asset");
    Console.WriteLine("2. View Assets");
    Console.WriteLine("3. Search Assets");
    Console.WriteLine("4. Remove Asset");
    Console.WriteLine("5. Exit");
    Console.Write("\nSelect option: ");

    string choice = Console.ReadLine() ?? "";
    Console.WriteLine();

    switch (choice)
    {
        case "1":
            {
                service.AddAsset();
                break;
            }
        case "2":
            {
                service.ShowAssets();
                break;
            }
        case "3":
            {
                service.SearchAssets();
                break;
            }
        case "4":
            {
                service.RemoveAsset();
                break;
            }
        case "5":
            {
                Console.WriteLine("Exiting...");
                return;
            }
        default:
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid option. Please try again.");
                Console.ResetColor();
                break;
            }
    }
}
