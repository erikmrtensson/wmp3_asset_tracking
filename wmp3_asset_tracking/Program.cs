using wmp3_asset_tracking.Models;
using wmp3_asset_tracking.Services;


AssetService service = new AssetService();

service.LoadAssets();
CurrencyService.FetchRates();

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
