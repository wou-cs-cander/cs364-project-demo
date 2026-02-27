# Database Interface Demonstration

At this time, I just have a demo program rather than any test cases.
It lives in the `demo` project, and you can run it with:
```
dotnet run --project demo
```

## Sample Output

```
dotnet run --project demo
Hello, World!

=== Listing Customers via CustomerRepository...
Customer #1: Larry Smith -  larry@stupidtuesday.com
Customer 1: Larry Smith (larry@stupidtuesday.com)
Customer 2: Lisa Simpson (lisa@harvard.edu)
Customer 3: Joe Average (huh@average.com)
Customer 4: Test Customer (test.customer@example.com)

=== Listing Customers via PartStoreContext...
Customer 1: Larry Smith (larry@stupidtuesday.com)
Customer 2: Lisa Simpson (lisa@harvard.edu)
Customer 3: Joe Average (huh@average.com)
Customer 4: Test Customer (test.customer@example.com)

=== Listing Stores via PartStoreContext...
Store 1: Monmouth (123 Main Street)
Store 2: Salem (666 Satan Ave)
Store 3: Portland (1313 Anarchy Way)
Store 4: Independence (456 Monmouth Ave)

=== Listing Inventories via PartStoreContext...
Inventory 1: Hubcap at Monmouth Store - Price: 12.34
Inventory 2: Trunk Latch at Monmouth Store - Price: 34.56
Inventory 3: Air Filter at Salem Store - Price: 12.99
Inventory 4: Hubcap at Salem Store - Price: 12.00
Inventory 5: Distributor Cap at Salem Store - Price: 5.99
Inventory 6: Spark Plug at Salem Store - Price: 6.99
Inventory 7: Starter Motor at Portland Store - Price: 62.49
Inventory 8: Air Filter at Portland Store - Price: 11.00
Inventory 9: Spark Plug at Monmouth Store - Price: 9.99
Inventory 10: Hubcap at Salem Store - Price: 60.39
Inventory 11: Hubcap at Monmouth Store - Price: 59.99
Inventory 12: Engine at Portland Store - Price: 2345.99
Inventory 13: Distributor Cap at Portland Store - Price: 56.99
Inventory 14: Spark Plug at Salem Store - Price: 13.99
Inventory 15: Oil Filter at Monmouth Store - Price: 19.99
Inventory 16: Cabin Air Fiter at Salem Store - Price: 35.99
Inventory 17: Starter Motor at Monmouth Store - Price: 79.99
Inventory 18: Starter Motor at Salem Store - Price: 69.99
Inventory 19: Hubcap at Portland Store - Price: 66.22
Inventory 20: Starter Motor at Salem Store - Price: 123.99

=== CRUD Operations via CustomerRepository ===
Created customer with ID: 15, Name: Test Customer, Email: test.customer@example.com
Retrieved customer: ID: 15, Name: Test Customer, Email: test.customer@example.com
Updated customer name to: Updated Test Customer
Verified update - Customer name is now: Updated Test Customer
Deleted customer with ID: 15

=== CRUD Operations via PartStoreContext ===
Created customer with ID: 16, Name: Context Test Customer, Email: context.test.customer@example.com
Retrieved customer: ID: 16, Name: Context Test Customer, Email: context.test.customer@example.com
Updated customer name to: Updated Context Test Customer
Verified update - Customer name is now: Updated Context Test Customer
Deleted customer with ID: 16
