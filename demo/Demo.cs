namespace Demo;

using Db.Ado;
using Models;
// If you get an error message like:
// DbSet<Inventory>' does not contain a definition for 'Include' and no
// accessible extension method 'Include' accepting a first argument of type
// 'DbSet<Inventory>' could be found ...
// you need EntityFrameworkCore below
using Microsoft.EntityFrameworkCore;

public class Demo
{
    private readonly string _connStr;

    public Demo(string connectionString)
    {
        _connStr = connectionString;
    }

    public void ListCustomersViaRepo()
    {
        Console.WriteLine("\n=== Listing Customers via CustomerRepository...");
        CustomerRepository repository = new CustomerRepository(_connStr);

        var cust = repository.GetById(1);
        Console.WriteLine($"Customer #1: {cust}");

        var customers = repository.GetAll();
        foreach (var c in customers)
        {
            Console.WriteLine($"Customer {c.CustomerId}: {c.Name} ({c.Email})");
        }
    }

    public void ListCustomersViaContext()
    {
        Console.WriteLine("\n=== Listing Customers via PartStoreContext...");
        using (var context = new PartStoreContext(_connStr))
        {
            var customers = context.Customers.ToList();
            foreach (var c in customers)
            {
                Console.WriteLine($"Customer {c.CustomerId}: {c.Name} ({c.Email})");
            }
        }
    }

    public void ListStoresViaContext()
    {
        Console.WriteLine("\n=== Listing Stores via PartStoreContext...");
        using (var context = new PartStoreContext(_connStr))
        {
            var stores = context.Stores.ToList();
            foreach (var s in stores)
            {
                Console.WriteLine($"Store {s.StoreId}: {s.StoreName} ({s.Address})");
            }
        }
    }

    public void ListInventoriesViaContext()
    {
        Console.WriteLine("\n=== Listing Inventories via PartStoreContext...");
        using (var context = new PartStoreContext(_connStr))
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

    public void DemoCrudOperationsViaRepo()
    {
        Console.WriteLine("\n=== CRUD Operations via CustomerRepository ===");
        CustomerRepository repository = new CustomerRepository(_connStr);

        var newCustomer = new Customer
        {
            Name = "Test Customer",
            Email = "test.customer@example.com"
        };
        repository.Add(newCustomer);
        Console.WriteLine($"Created customer with ID: {newCustomer.CustomerId}, Name: {newCustomer.Name}, Email: {newCustomer.Email}");

        var retrievedCustomer = repository.GetById(newCustomer.CustomerId);
        Console.WriteLine($"Retrieved customer: ID: {retrievedCustomer.CustomerId}, Name: {retrievedCustomer.Name}, Email: {retrievedCustomer.Email}");

        retrievedCustomer.Name = "Updated Test Customer";
        repository.Update(retrievedCustomer);
        Console.WriteLine($"Updated customer name to: {retrievedCustomer.Name}");

        var updatedCustomer = repository.GetById(retrievedCustomer.CustomerId);
        Console.WriteLine($"Verified update - Customer name is now: {updatedCustomer.Name}");

        repository.Delete(updatedCustomer.CustomerId);
        Console.WriteLine($"Deleted customer with ID: {updatedCustomer.CustomerId}");
    }

    public void DemoCrudOperationsViaContext()
    {
        Console.WriteLine("\n=== CRUD Operations via PartStoreContext ===");

        using (var context = new PartStoreContext(_connStr))
        {
            var newCustomer = new Customer
            {
                Name = "Context Test Customer",
                Email = "context.test.customer@example.com"
            };

            context.Customers.Add(newCustomer);
            context.SaveChanges();
            Console.WriteLine($"Created customer with ID: {newCustomer.CustomerId}, Name: {newCustomer.Name}, Email: {newCustomer.Email}");

            var retrievedCustomer = context.Customers.FirstOrDefault(c => c.CustomerId == newCustomer.CustomerId);
            if (retrievedCustomer is null)
            {
                Console.WriteLine("Unable to read newly created customer.");
                return;
            }

            Console.WriteLine($"Retrieved customer: ID: {retrievedCustomer.CustomerId}, Name: {retrievedCustomer.Name}, Email: {retrievedCustomer.Email}");

            retrievedCustomer.Name = "Updated Context Test Customer";
            context.SaveChanges();
            Console.WriteLine($"Updated customer name to: {retrievedCustomer.Name}");

            var updatedCustomer = context.Customers.FirstOrDefault(c => c.CustomerId == retrievedCustomer.CustomerId);
            if (updatedCustomer is null)
            {
                Console.WriteLine("Unable to re-read updated customer.");
                return;
            }

            Console.WriteLine($"Verified update - Customer name is now: {updatedCustomer.Name}");

            context.Customers.Remove(updatedCustomer);
            context.SaveChanges();
            Console.WriteLine($"Deleted customer with ID: {updatedCustomer.CustomerId}");
        }
    }
}
