namespace Demo;

using Models;
using Microsoft.EntityFrameworkCore;

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

    public void DeleteCustomer(int customerId)
    {
        using var context = new PartStoreContext(_connectionString);

        var customer = context.Customers.Find(customerId);
        if (customer == null)
        {
            Console.WriteLine($"Customer with ID {customerId} not found.");
            return;
        }

        context.Customers.Remove(customer);
        context.SaveChanges();

        Console.WriteLine($"Deleted customer: {customer.CustomerId} - {customer.Name} ({customer.Email})");
    }

    public void SearchForItemInventory(int itemId)
    {
        using var context = new PartStoreContext(_connectionString);

        var invs = context.Inventories
            .Where(i => i.ItemId == itemId)
            .OrderBy(i => i.Price)
            .Include(i => i.Store)
            .Select(i => new
            {
                StoreName = i.Store.StoreName,
                Quantity = i.Quantity,
                Price = i.Price
            })
            .ToList();
        Console.WriteLine($"Inventory for item {itemId}, sorted by ascending price:");
        foreach (var item in invs)
        {
            Console.WriteLine($"Store: {item.StoreName}, Quantity: {item.Quantity}, Price: ${item.Price:F2}");
        }
    }
}
