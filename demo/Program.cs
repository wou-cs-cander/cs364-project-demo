namespace Demo;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        string connStr = GetConnectionString(args);

        // TestConnection(connStr);

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
