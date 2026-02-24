# Scaffolding

Scaffolding is how to generate model classes from an existing database using
EF Core.

Here is the official documentation:
https://learn.microsoft.com/en-us/ef/core/managing-schemas/scaffolding/?tabs=dotnet-core-cli

Here's the command I ran from the top-level project directory to create the
models.  Note, you need a connection string that's appropriate for your set
up of SQL Server (not mine). It should be similar to same one you use with Azure
Data Studio but you'll want to specify your database in the `Initial
Catalog` parameter.

```
dotnet ef dbcontext scaffold
    'Server=localhost;User Id=SA;Password=XXX;TrustServerCertificate=true;Initial Catalog=PartStore'
    Microsoft.EntityFrameworkCore.SqlServer
    --namespace Models --project db.efc
```
(Note, this command is all on one line. I broke it up to make it easier to
read here.)

You have to use `db.efc` for the project because it has the EF Core dependencies.

However, this generated the files in `db.efc` because that's the project we
told it to use.
The namespace for the classes is correct - `Models`.

I tested that everything built correctly by running `dotnet build`.
(Note that it throws a warning about the connection string being hardcoded
in the source file that it generates for the `DbContext` class. That's OK,
but you can quiet it by editing/removing the `#warning` in the code.)

Then, I moved the model files over to the `Models` directory. However, leave
the `DbContext` file in the `db.efc` project - it is genuinely dependent on
EF Core. So, all that's left in that project is the `DbContext` class.

Before hacking on things further, check everything still compiles. Now,
commit the C# files to GitHub.

You have a first draft. 🎉
Now, get hacking!
