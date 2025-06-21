
using BusinessObjects;
using DataAccessLayer;

namespace Repositories;
public class CategoryRepository : ICatergoryRepository {
  private CategoryDAO dao = new CategoryDAO();

  public void AddCategory(Category category) {
    dao.AddItem(category);
  }

  public void DeleteCategory(Category category) {
    dao.RemoveItem(category);
  }

  public void DeleteCategory(int id) {
    dao.RemoveItem(id);
  }

  public List<Category> GetCategories() {
    return dao.GetAllItems();
  }

  public Category? GetCategory(int id) {
    return dao.GetItem(id);
  }

  public void UpdateCategory(Category category) {
    dao.UpdateItem(category);
  }
}
