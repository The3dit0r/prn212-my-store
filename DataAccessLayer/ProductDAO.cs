using Microsoft.EntityFrameworkCore;
using BusinessObjects;

namespace DataAccessLayer;
public class ProductDAO : _BaseDAO<Product> {
  public ProductDAO() : base(new DatabaseContext().Products) { }

  public Product? GetItem(int id) {
    return GetItem((item) => item.ProductId == id);
  }

  public void RemoveItem(int id) {
    RemoveItems((item) => item.ProductId == id);
  }
}
