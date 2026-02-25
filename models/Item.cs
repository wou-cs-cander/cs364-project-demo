using System;
using System.Collections.Generic;

namespace Models;

public partial class Item
{
    public int ItemId { get; set; }

    public string Description { get; set; } = null!;

    public string MgfName { get; set; } = null!;

    public int Year { get; set; }

    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

    public override string ToString()
    {
        return $"{MgfName} -  {Description}";
    }
}
