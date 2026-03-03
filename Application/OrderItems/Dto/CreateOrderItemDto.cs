using System;
using System.Collections.Generic;
using System.Text;

namespace Application.OrderItems.Dto;

public class CreateOrderItemDto
{
    public decimal UnitPrice { get; set; }
    public string ProductName { get; set; } = null!;
    public int Quantity { get; set; }
    public string StockKeepingUnit { get; set; } = null!;
}
