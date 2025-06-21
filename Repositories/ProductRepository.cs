using BusinessObjects;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories;
public class ProductRepository : IProductRepository {
  ProductDAO dao = new ProductDAO();
  public void AddProduct(Product p) {
    dao.AddItem(p);
  }

  public void DeleteProduct(Product p) {
    dao.RemoveItem(p);
  }

  public void DeleteProduct(int id) {
    dao.RemoveItem(id);
  }

  public Product? GetProduct(int id) {
    return dao.GetItem(id);
  }

  public List<Product> GetProducts() {
    return dao.GetAllItems();
  }

  public void UpdateProduct(Product p) {
    dao.UpdateItem(p);
  }
}

