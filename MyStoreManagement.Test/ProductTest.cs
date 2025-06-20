using BusinessObjects;
using DataAccessLayer;
using System;

namespace MyStoreManagement.Test;

public class Products {
  private readonly ProductDAO DAO;

  public Products() {
    DAO = new ProductDAO();
  }

  [Fact]
  public void GetProducts_ShouldReturnProducts() {
    var products = DAO.GetItems();
    Assert.NotNull(products);
  }

  [Fact]
  public void GetProduct_ShouldReturnProduct() {
    int TAR = 5;

    var product = DAO.GetItem(TAR);
    Assert.NotNull(product);
    Assert.Equal(product.ProductId, TAR);
  }

  [Fact]
  public void GetProduct_ShouldReturnNull() {
    int TAR = 1000;
    var product = DAO.GetItem(TAR);
    Assert.Null(product);
  }

  [Fact(Skip = "Identity Crisis")]
  public void AddProduct_ShouldAddProduct() {
    int TAR = 28;

    //DAO.AddItem("The cat is in the bag", 11, 249, 1, TAR);
    var product = DAO.GetItem(TAR);

    Assert.NotNull(product);
    Assert.Equal(product.ProductId, TAR);
  }

  [Fact]
  public void AddProduct_ShouldRemoveProduct() {
    int TAR = 28;

    DAO.RemoveItem(TAR);
    var product = DAO.GetItem(TAR);

    Assert.Null(product);
  }
}
