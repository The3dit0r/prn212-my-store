using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class Product {
  public int ProductId { get; set; }

  public string ProductName { get; set; } = null!;

  public int? CategoryId { get; set; }

  public short? UnitsInStock { get; set; }

  public decimal? UnitPrice { get; set; }

  public virtual Category? Category { get; set; }

  public static bool IsDifferent(Product? t, Product? o) {
    if (o == null) return true;
    if (t == null) return true;

    return t.ProductId != o.ProductId
        || t.ProductName != o.ProductName
        || t.CategoryId != o.CategoryId
        || t.UnitsInStock != o.UnitsInStock
        || t.UnitPrice != o.UnitPrice;
  }
}
