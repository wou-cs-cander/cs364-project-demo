namespace Demo;

using Db.Ado;
using Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
// If you get an error message like:
// DbSet<Inventory>' does not contain a definition for 'Include' and no
// accessible extension method 'Include' accepting a first argument of type
// 'DbSet<Inventory>' could be found ...
// you need EntityFrameworkCore below
using Microsoft.EntityFrameworkCore;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        string connStr = GetConnectionString(args);

        // TestConnection(connStr);
        ListCustomersViaRepo(connStr);
        ListCustomersViaContext(connStr);
        ListStoresViaContext(connStr);
        ListInventoriesViaContext(connStr);
        DemoCrudOperationsViaRepo(connStr);
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

    static void ListCustomersViaRepo(string connStr)
    {
        Console.WriteLine("\n=== Listing Customers via CustomerRepository...");
        CustomerRepository repository = new CustomerRepository(connStr);

        var cust = repository.GetById(1);
        Console.WriteLine($"Customer #1: {cust}");

        var customers = repository.GetAll();
        foreach (var c in customers)
        {
            Console.WriteLine($"Customer {c.CustomerId}: {c.Name} ({c.Email})");
        }
    }

    static void ListCustomersViaContext(string connStr)
    {
        Console.WriteLine("\n=== Listing Customers via PartStoreContext...");
        using (var context = new PartStoreContext(connStr))
        {
            var customers = context.Customers.ToList();
            foreach (var c in customers)
            {
                Console.WriteLine($"Customer {c.CustomerId}: {c.Name} ({c.Email})");
            }
        }
    }

    static void ListStoresViaContext(string connStr)
    {
        Console.WriteLine("\n=== Listing Stores via PartStoreContext...");
        using (var context = new PartStoreContext(connStr))
        {
            var stores = context.Stores.ToList();
            foreach (var s in stores)
            {
                Console.WriteLine($"Store {s.StoreId}: {s.StoreName} ({s.Address})");
            }
        }
    }

    static void ListInventoriesViaContext(string connStr)
    {
        Console.WriteLine("\n=== Listing Inventories via PartStoreContext...");
        using (var context = new PartStoreContext(connStr))
        {
            // NB: need to use Include() to eagerly load the related Item and Store entities,
            // otherwise they will be null.  The alternative is to use lazy loading proxies
            // in the DbContext configuration.
            var inventories = context.Inventories
                            .Include(i => i.Item).Include(i => i.Store)
                            .ToList();
            foreach (var i in inventories)
            {
                Console.WriteLine($"Inventory {i.InventoryId}: {i.Item.Description} at {i.Store.StoreName} Store - Price: {i.Price:F2}");
            }
        }
    }

    static void DemoCrudOperationsViaRepo(string connStr)
    {
        Console.WriteLine("\n=== CRUD Operations via CustomerRepository ===");
        CustomerRepository repository = new CustomerRepository(connStr);

        // CREATE: Add a new customer
        var newCustomer = new Customer
        {
            Name = "Test Customer",
            Email = "test.customer@example.com"
        };
        repository.Add(newCustomer);
        Console.WriteLine($"Created customer with ID: {newCustomer.CustomerId}, Name: {newCustomer.Name}, Email: {newCustomer.Email}");

        // READ: Get the customer back from the database
        var retrievedCustomer = repository.GetById(newCustomer.CustomerId);
        Console.WriteLine($"Retrieved customer: ID: {retrievedCustomer.CustomerId}, Name: {retrievedCustomer.Name}, Email: {retrievedCustomer.Email}");

        // UPDATE: Change the customer's name
        retrievedCustomer.Name = "Updated Test Customer";
        repository.Update(retrievedCustomer);
        Console.WriteLine($"Updated customer name to: {retrievedCustomer.Name}");

        // READ: Verify the update
        var updatedCustomer = repository.GetById(retrievedCustomer.CustomerId);
        Console.WriteLine($"Verified update - Customer name is now: {updatedCustomer.Name}");

        // DELETE: Remove the test customer
        repository.Delete(updatedCustomer.CustomerId);
        Console.WriteLine($"Deleted customer with ID: {updatedCustomer.CustomerId}");
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
