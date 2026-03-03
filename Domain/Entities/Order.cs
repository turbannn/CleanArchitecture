using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities;

public class Order
{
    public Guid Id { get; set; }
    public DateTime OrderDate { get; set; }
    public string ShippingAddress { get; set; } = null!;
    public string Notes { get; set; } = null!;
    public decimal TotalPrice => Items.Sum(x => x.UnitPrice);

    public List<OrderItem> Items { get; set; } = null!;
}
