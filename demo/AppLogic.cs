namespace Demo;

public class AppLogic
{
    private readonly string _connectionString;

    public AppLogic(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void Hello()
    {
        Console.WriteLine(_connectionString);
    }
}
