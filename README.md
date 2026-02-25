# Awesome Auto Search

Welcome to the home of the Awesome Auto Search application.

Stay tuned for more details

## Demo database connection configuration

The demo app now reads the database connection string from configuration (not from source code).

Preferred option: .NET user secrets

```bash
dotnet user-secrets --project demo set "ConnectionStrings:PartStore" "Server=localhost;User Id=UUU;Password=PPP;TrustServerCertificate=true;Initial Catalog=PartStore"
```

Alternative option: environment variable

```bash
export PARTSTORE_CONNECTION_STRING="Server=localhost;User Id=SA;Password=reallyStrongPwd123;TrustServerCertificate=true;Initial Catalog=PartStore"
```

Then run:

```bash
dotnet run --project demo
```
