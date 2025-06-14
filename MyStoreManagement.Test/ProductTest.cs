using BusinessObjects;
using DataAccessLayer;
using System;

namespace MyStoreManagement.Test;

public class Products {
  [Fact]
  public void GetProducts_ShouldReturnProducts() {
    var DAO = new ProductDAO();
    var products = DAO.GetProducts();

    Assert.NotEmpty(products);
  }

  [Fact]
  public void GetProduct_ShouldReturnProduct() {
    int TAR = 5;

    var DAO = new ProductDAO();
    var product = DAO.GetProduct(TAR);

    Assert.NotNull(product);
    Assert.Equal(product.ProductId, TAR);
  }

  [Fact]
  public void GetProduct_ShouldReturnNull() {
    int TAR = -1;

    var DAO = new ProductDAO();
    var product = DAO.GetProduct(TAR);

    Assert.Null(product);
  }
}
