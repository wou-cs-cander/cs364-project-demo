namespace Demo;

//using System.Runtime.CompilerServices;
using Db.Ado;
using Models;
using Microsoft.Data.SqlClient;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        // NEVER hardcode the connection string in your code like this. This is just for demo purposes.
        // My connection string is for SQL Server running in a Docker container on my local machine.
        // You will need to change this to match your own database connection string.
        // The Initial Catalog is the name of the database you want to connect to. In this case, it's "PartStore".
        string connStr = "Server=localhost;User Id=SA;Password=reallyStrongPwd123;TrustServerCertificate=true;Initial Catalog=PartStore";

        // TestConnection(connStr);

        CustomerRepository repository = new CustomerRepository(connStr);

        var Customer = repository.GetById(1);
        Console.WriteLine($"Customer: {Customer.Name} ({Customer.Email})");
    }

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
}
