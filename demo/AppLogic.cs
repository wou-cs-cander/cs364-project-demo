namespace Demo;

using Models;

public class AppLogic
{
    private readonly string _connectionString;

    public AppLogic(string connectionString)
    {
        _connectionString = connectionString;
    }

    public int AddCustomer(string customerName, string email)
    {
        using var context = new PartStoreContext(_connectionString);

        bool exists = context.Customers.Any(c => c.Email == email);
        if (exists)
        {
            Console.WriteLine($"Customer with email {email} already exists.");
            return -1;
        }

        var customer = new Customer
        {
            Name = customerName,
            Email = email
        };

        context.Customers.Add(customer);
        context.SaveChanges();

        Console.WriteLine($"Inserted new customer: {customer.CustomerId} - {customer.Name} ({customer.Email})");

        return customer.CustomerId;
    }
}
