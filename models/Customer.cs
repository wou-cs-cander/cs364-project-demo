namespace Models;

public class Customer : EntityBase
{
    public int CustomerId { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public override string ToString()
    {
        return $"{Name} -  {Email}";
    }
}
