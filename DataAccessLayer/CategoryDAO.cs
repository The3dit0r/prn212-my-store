using Microsoft.EntityFrameworkCore;
using BusinessObjects;

namespace DataAccessLayer;
public class CategoryDAO: _BaseDAO<Category> {
  public CategoryDAO(DbSet<Category> dbset): base(dbset) {}

  public Category? GetItem(int id) {
    return GetItem((item) => item.CategoryId == id);
  }

  public void RemoveItem(int id) {
    RemoveItems((item) => item.CategoryId == id);
  }
}
