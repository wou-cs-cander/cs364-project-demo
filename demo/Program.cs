namespace Demo;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Models;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length != 1)
        {
            PrintUsage();
            return;
        }

        string command = args[0].Trim().ToLowerInvariant();
        string connStr = GetConnectionString(Array.Empty<string>());

        switch (command)
        {
            case "app":
                RunApplication(connStr);
                break;
            case "demo":
                RunCrudDemo(connStr);
                break;
            default:
                PrintUsage();
                break;
        }
    }

    static void PrintUsage()
    {
        Console.WriteLine("Usage: dotnet run --project demo -- <app|demo>");
        Console.WriteLine("  app  - run the application workflow");
        Console.WriteLine("  demo - run the CRUD demonstration");
    }

    static void RunApplication(string connectionString)
    {
        var appLogic = new AppLogic(connectionString);

        int newCustomerId = appLogic.AddCustomer("Testy McTestFace", "test@test.com");
        if (newCustomerId < 0)
        {
            Console.WriteLine("Failed to add customer. Aborting further operations.");
            return;
        }

        appLogic.SearchForItemInventory(1);

        // IDs from the seed data
        const int monmouth = 1;
        const int hubcap = 1;
        const int airfilter = 9;
        const int larry = 1;
        List<int> itemIds = new List<int> { hubcap, airfilter };

        // Don't use newCustomerId for the order since we delete the customer
        // at the end and FK constraints won't allow us to delete a customer with orders.
        var order = appLogic.PlaceOrder(monmouth, larry, itemIds);
        if (order != null)
        {
            Console.WriteLine($"Order {order.OrderId} placed successfully for customer {larry} at store {monmouth}.");
        }
        else
        {
            Console.WriteLine("Failed to place order.");
        }

        appLogic.DeleteCustomer(newCustomerId);
    }

    static void RunCrudDemo(string connStr)
    {
        Console.WriteLine("=== CRUD Demonstration ===");
        var demo = new Demo(connStr);

        demo.ListCustomersViaRepo();
        demo.ListCustomersViaContext();
        demo.ListStoresViaContext();
        demo.ListInventoriesViaContext();
        demo.DemoCrudOperationsViaRepo();
        demo.DemoCrudOperationsViaContext();

    }

    // Just a quick test to see if we can connect to the database and get some info about the connection.
    // I used it while debugging issues with my connection string and database setup.
    static void TestConnection(string connectionString)
    {
        using var connection = new SqlConnection(connectionString);
        connection.Open();
        Console.WriteLine("Connection successful!");
        Console.WriteLine("Database: {0}", connection.Database);
        Console.WriteLine("State: {0}", connection.State);

        connection.ChangeDatabase("PartStore");
        Console.WriteLine("Database: {0}", connection.Database);

        Console.WriteLine("ConnectionString: {0}",
            connection.ConnectionString);
    }

    //
    // This is the "fancy" way to get a DB connection string.
    // Students can just hard-code the connection string if they want for simplicity's sake,
    // but this is a more flexible approach that allows for multiple configuration sources (appsettings.json, user secrets, environment variables, command-line args).
    static string GetConnectionString(string[] args)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
            .AddEnvironmentVariables()
            .AddUserSecrets<Program>(optional: true)
            .AddCommandLine(args)
            .Build();

        string? connStr =
            config.GetConnectionString("PartStore")
            ?? config["ConnectionStrings:PartStore"]
            ?? config["PARTSTORE_CONNECTION_STRING"];

        if (string.IsNullOrWhiteSpace(connStr))
        {
            throw new InvalidOperationException(
                "Connection string not found. Configure 'ConnectionStrings:PartStore' via user secrets, appsettings.json, or environment variable 'PARTSTORE_CONNECTION_STRING'.");
        }

        return connStr;
    }
}
