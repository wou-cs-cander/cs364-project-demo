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
Listing Customers via CustomerRepository...
Customer #1: Larry Smith -  larry@stupidtuesday.com
Customer 1: Larry Smith (larry@stupidtuesday.com)
Customer 2: Lisa Simpson (lisa@harvard.edu)
Customer 3: Joe Average (huh@average.com)
Listing Customers via PartStoreContext...
Customer 1: Larry Smith (larry@stupidtuesday.com)
Customer 2: Lisa Simpson (lisa@harvard.edu)
Customer 3: Joe Average (huh@average.com)
Listing Stores via PartStoreContext...
Store 1: Monmouth (123 Main Street)
Store 2: Salem (666 Satan Ave)
Store 3: Portland (1313 Anarchy Way)
Store 4: Independence (456 Monmouth Ave)
```
