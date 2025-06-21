using BusinessObjects;

namespace Services;
public interface IProductService {
  void AddProduct(Product p);

  void DeleteProduct(Product p);
  void DeleteProduct(int id);

  void UpdateProduct(Product p);

  List<Product> GetProducts();

  Product? GetProduct(int id);
}
