# Setting Up a .NET 9 C# Project Hierarchy with the `dotnet` CLI

This tutorial walks through creating a solution with five projects — `models`, `db.ado`, `db.efc`, `demo`, and `test` — all wired together with the right dependencies.

---

## 1. Create the Solution
In your project repository directory:

```bash
dotnet new sln -n MyApp
```

This creates `MyApp.sln` in the current directory. All projects will live here alongside it.
(Please use a better name than `MyApp`.)

---

## 2. Create the Projects

### `models` — Plain class library, no special dependencies

```bash
dotnet new classlib -n models -f net9.0
```

### `db.ado` — ADO.NET with SQL Server

```bash
dotnet new classlib -n db.ado -f net9.0
```

### `db.efc` — Entity Framework Core with SQL Server

```bash
dotnet new classlib -n db.efc -f net9.0
```

### `demo` — Console application

```bash
dotnet new console -n demo -f net9.0
```

### `test` — xUnit test project

```bash
dotnet new xunit -n test -f net9.0
```

---

## 3. Add All Projects to the Solution

```bash
dotnet sln add models/models.csproj
dotnet sln add db.ado/db.ado.csproj
dotnet sln add db.efc/db.efc.csproj
dotnet sln add demo/demo.csproj
dotnet sln add test/test.csproj
```

---

## 4. Add NuGet Package Dependencies

### `db.ado` — needs the SQL Server ADO.NET driver

```bash
dotnet add db.ado/db.ado.csproj package Microsoft.Data.SqlClient
```

### `db.efc` — needs EF Core and its SQL Server provider
We are specifying the version (9.0.13) to be compatible with version 9.0 of .NET that we're running.

```bash
dotnet add db.efc/db.efc.csproj package Microsoft.EntityFrameworkCore --version 9.0.13
dotnet add db.efc/db.efc.csproj package Microsoft.EntityFrameworkCore.SqlServer --version 9.0.13
dotnet add db.efc/db.efc.csproj package Microsoft.EntityFrameworkCore.Design --version 9.0.13
```

> `Microsoft.EntityFrameworkCore.Design` is needed if you plan to use EF migrations (`dotnet ef migrations add ...`).

---

## 5. Wire Up Project References

### `db.ado` and `db.efc` both depend on `models`

```bash
dotnet add db.ado/db.ado.csproj reference models/models.csproj
dotnet add db.efc/db.efc.csproj reference models/models.csproj
```

### `demo` depends on all three libraries

```bash
dotnet add demo/demo.csproj reference models/models.csproj
dotnet add demo/demo.csproj reference db.ado/db.ado.csproj
dotnet add demo/demo.csproj reference db.efc/db.efc.csproj
```

### `test` depends on all three libraries it needs to test

```bash
dotnet add test/test.csproj reference models/models.csproj
dotnet add test/test.csproj reference db.ado/db.ado.csproj
dotnet add test/test.csproj reference db.efc/db.efc.csproj
```

> The `xunit` template already includes the `xunit` and `xunit.runner.visualstudio` packages. No extra steps needed for the test runner.

---

## 6. Verify the Structure

After running all the commands, your directory should look like this:

```
MyApp/
├── MyApp.sln
├── models/
│   ├── models.csproj
│   └── Class1.cs
├── db.ado/
│   ├── db.ado.csproj
│   └── Class1.cs
├── db.efc/
│   ├── db.efc.csproj
│   └── Class1.cs
├── demo/
│   ├── demo.csproj
│   └── Program.cs
└── test/
    ├── test.csproj
    └── UnitTest1.cs
```

You can confirm everything resolves correctly by building the whole solution:

```bash
dotnet build
```

And run the tests with:

```bash
dotnet test
```

---

## Quick Reference: Dependency Map

```
models
  ├── db.ado  (+ Microsoft.Data.SqlClient)
  │     └── demo
  ├── db.efc  (+ EF Core + EF SqlServer + EF Design)
  │     └── demo
  └── test    (tests models, db.ado, db.efc)
```

---

## Useful dotnet Commands Cheat Sheet

| Task | Command |
|---|---|
| List solution projects | `dotnet sln list` |
| List project references | `dotnet list <proj> reference` |
| List project packages | `dotnet list <proj> package` |
| Run the demo app | `dotnet run --project demo` |
| Run tests | `dotnet test` |
| Add EF migrations tool | `dotnet tool install --global dotnet-ef` |
| Add a migration | `dotnet ef migrations add Init --project db.efc` |
