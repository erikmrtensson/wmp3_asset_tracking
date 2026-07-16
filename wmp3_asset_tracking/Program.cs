using wmp3_asset_tracking.Data;
using wmp3_asset_tracking.Models;
using wmp3_asset_tracking.Services;


using var context = new AssetContext();
AssetService service = new AssetService(context);

service.SeedDatabase();
CurrencyService.FetchRates();

while (true)
{
    Console.WriteLine("==================================================");
    Console.WriteLine("         COMPANY ASSET TRACKING SYSTEM           ");
    Console.WriteLine("==================================================");
    Console.WriteLine("1. Add Asset");
    Console.WriteLine("2. View Assets");
    Console.WriteLine("3. Search / Filter");
    Console.WriteLine("4. Update Asset");
    Console.WriteLine("5. Remove Asset");
    Console.WriteLine("6. Show Report");
    Console.WriteLine("7. Export data");
    Console.WriteLine("8. Exit");
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
                service.SearchFilterMenu();
                break;
            }
        case "4":
            {
                service.UpdateAsset();
                break;
            }
        case "5":
            {
                service.RemoveAsset();
                break;
            }
        case "6":
            {
                service.ShowReport();
                break;
            }
        case "7":
            {
                service.ExportMenu();
                break;
            }
        case "8":
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
