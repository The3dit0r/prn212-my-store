
using BusinessObjects;

namespace Repositories;
public interface IProductRepository {
  void AddProduct(Product p);

  void DeleteProduct(Product p);
  void DeleteProduct(int id);

  void UpdateProduct(Product p);

  List<Product> GetProducts();

  Product? GetProduct(int id);
}
