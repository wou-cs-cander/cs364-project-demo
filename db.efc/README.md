
# Scaffolding

Documentation
https://learn.microsoft.com/en-us/ef/core/managing-schemas/scaffolding/?tabs=dotnet-core-cli


```
dotnet ef dbcontext scaffold 'Server=localhost;User Id=SA;Password=XXX;TrustServerCertificate=true;Initial Catalog=PartStore' Microsoft.EntityFrameworkCore.SqlServer --namespace Models --project db.efc
```

Gotta use `db.efc` for the project because it has the EF Core dependencies.

This generated the files in `db.efc` because that's the project. The namespace
for the classes is correct - `Models`.
I want to move the generated models over to the models project
The generated classes are partial, which doesn't matter to me.
Before hacking on it, everything compiles - with warnings about the
connection string in the DbContext
