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

    /// <summary>
    /// Place an order at a store for a customer with a list of item IDs.
    /// Will check that the store and customer exist, and that the store has
    /// sufficient inventory of each item before placing the order.  We assume
    /// that the quatity for each item is 1 for simplicity.
    /// If any checks fail, will return null and not place the order.
    /// </summary>
    /// <param name="storeId"></param>
    /// <param name="customerId"></param>
    /// <param name="itemIds"></param>
    /// <returns></returns>
    public Order? PlaceOrder(int storeId, int customerId, List<int> itemIds)
    {
        using var context = new PartStoreContext(_connectionString);

        if (itemIds == null || itemIds.Count == 0)
        {
            Console.WriteLine("Cannot place an order with no items.");
            return null;
        }

        bool storeExists = context.Stores.Any(s => s.StoreId == storeId);
        if (!storeExists)
        {
            Console.WriteLine($"Store with ID {storeId} not found.");
            return null;
        }

        bool customerExists = context.Customers.Any(c => c.CustomerId == customerId);
        if (!customerExists)
        {
            Console.WriteLine($"Customer with ID {customerId} not found.");
            return null;
        }

        decimal orderTotal = 0m;
        var order = new Order
        {
            StoreId = storeId,
            CustomerId = customerId,
            Completed = 0
        };

        var requestedCounts = itemIds
            .GroupBy(id => id)
            .ToDictionary(group => group.Key, group => group.Count());

        foreach (var requestedItem in requestedCounts)
        {
            int itemId = requestedItem.Key;
            int requestedQuantity = requestedItem.Value;

            var inventory = context.Inventories
                .FirstOrDefault(i => i.StoreId == storeId && i.ItemId == itemId);

            if (inventory == null || inventory.Quantity < requestedQuantity)
            {
                Console.WriteLine(
                    $"Cannot place order: item {itemId} has insufficient quantity at store {storeId}.");
                return null;
            }

            for (int i = 0; i < requestedQuantity; i++)
            {
                // TODO: should also be decreasing the inventory quantity here
                // Not updating inventory quantity makes testing easier - don't
                // need to keep resetting the database to have inventory.
                order.OrderItems.Add(new OrderItem
                {
                    ItemId = itemId,
                    Quantity = 1,
                    TotalAmount = inventory.Price
                });
                orderTotal += inventory.Price;
            }
        }

        order.TotalAmount = orderTotal;

        context.Orders.Add(order);
        context.SaveChanges();

        Console.WriteLine(
            $"Created order {order.OrderId} for customer {customerId} at store {storeId} with {order.OrderItems.Count} items.");

        return order;
    }
}
