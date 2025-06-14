using BusinessObjects;

namespace DataAccessLayer;
public class ProductDAO {
  public List<Product> GetProducts() {
    var list = new List<Product>();

    using var context = new DatabaseContext();
    list = context.Products.ToList();

    return list;
  }

  public Product? GetProduct(int id) {
    Product? product = null;

    using var context = new DatabaseContext();
    product = context.Products.Where(prod => prod.ProductId == id).FirstOrDefault();

    return product;
  }
}
