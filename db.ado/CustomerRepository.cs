
using Microsoft.Data.SqlClient;
using Models;

namespace Db.Ado;

public class CustomerRepository : IRepository<Customer>
{
    private readonly string? _connectionString;
    public CustomerRepository(string connectionString)
    {
        _connectionString = connectionString;
    }
    public void Add(Customer entity)
    {
        using var connection = new SqlConnection(_connectionString);

        var sql = @"INSERT INTO Customers ([Name], [Email])
                    OUTPUT INSERTED.[CustomerId]
                    VALUES (@Name, @Email);";
        var command = new SqlCommand(sql, connection);

        command.Parameters.AddWithValue("@Name", entity.Name);
        command.Parameters.AddWithValue("@Email", entity.Email);

        connection.Open();

        // Otherwise, get the ID of the newly inserted customer
        var id = (int)command.ExecuteScalar();
        entity.CustomerId = id;

        connection.Close();
    }

    public void Delete(int id)
    {
        using var connection = new SqlConnection(_connectionString);

        var sql = "DELETE FROM Customers WHERE [CustomerId] = @Id;";
        var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Id", id);

        connection.Open();
        command.ExecuteNonQuery();
        connection.Close();
    }

    public IEnumerable<Customer> GetAll()
    {
        var customers = new List<Customer>();
        var sql = "SELECT * FROM Customers;";

        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        var command = new SqlCommand(sql, connection);
        var reader = command.ExecuteReader();

        while (reader.Read())
        {
            var customer = new Customer
            {
                CustomerId = (int)reader["CustomerId"],
                Name = reader["Name"] as string,
                Email = reader["Email"] as string
            };

            customers.Add(customer);
        }

        connection.Close();
        return customers;
    }

    public Customer GetById(int id)
    {
        using var connection = new SqlConnection(_connectionString);

        var command = new SqlCommand("SELECT * FROM Customers WHERE [CustomerId] = @Id;", connection);
        command.Parameters.AddWithValue("@Id", id);

        connection.Open();

        var reader = command.ExecuteReader();
        reader.Read();

        var customer = new Customer
        {
            CustomerId = (int)reader["CustomerId"],
            Name = reader["Name"] as string,
            Email = reader["Email"] as string,
        };

        return customer;
    }

    public void Update(Customer entity)
    {
        var sql = @"UPDATE Customers
                    SET [Name] = @Name,
                        [Email] = @Email
                    WHERE [CustomerId] = @Id;";

        using var connection = new SqlConnection(_connectionString);
        var command = new SqlCommand(sql, connection);

        command.Parameters.AddWithValue("@Id", entity.CustomerId);
        command.Parameters.AddWithValue("@Name", entity.Name);
        command.Parameters.AddWithValue("@Email", entity.Email);

        connection.Open();
        command.ExecuteNonQuery();
        connection.Close();
    }
}
