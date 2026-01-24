using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities;

public class Order
{
    public Guid Id { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalPrice => Items.Sum(x => x.UnitPrice);

    public List<OrderItem> Items { get; set; }
}
