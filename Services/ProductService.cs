using BusinessObjects;
using Repositories;

namespace Services;
public class ProductService : IProductService {
  private readonly IProductRepository repo;

  public ProductService() {
    repo = new ProductRepository();
  }

  public void AddProduct(Product p) {
    repo.AddProduct(p);
  }

  public void DeleteProduct(Product p) {
    repo.DeleteProduct(p);
  }

  public void DeleteProduct(int id) {
    repo.DeleteProduct(id);
  }

  public Product? GetProduct(int id) {
    return repo.GetProduct(id);
  }

  public List<Product> GetProducts() {
    return repo.GetProducts();
  }

  public void UpdateProduct(Product p) {
    repo.UpdateProduct(p);
  }
}
