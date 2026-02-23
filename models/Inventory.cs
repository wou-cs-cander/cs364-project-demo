using System;
using System.Collections.Generic;

namespace Models;

public partial class Inventory
{
    public int InventoryId { get; set; }

    public int ItemId { get; set; }

    public int StoreId { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public virtual Item Item { get; set; } = null!;

    public virtual Store Store { get; set; } = null!;
}
