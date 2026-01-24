using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities;

public class OrderItem
{
    public Guid Id { get; set; }
    public decimal UnitPrice { get; set; }
    public string ProductName { get; set; } = null!;
    public int Quantity { get; set; }
    //public Guid OrderId { get; set; }
}