using BusinessObjects;
using Repositories;

namespace Services;
public class CategoryService: ICategoryService {
  private readonly ICatergoryRepository repo;

  public CategoryService() {
    repo = new CategoryRepository();
  }

  public void AddCategory(Category category) {
    repo.AddCategory(category);
  }

  public void DeleteCategory(Category category) {
    repo.DeleteCategory(category);
  }

  public void DeleteCategory(int id) {
    repo.DeleteCategory(id);
  }

  public List<Category> GetCategories() {
    return repo.GetCategories();
  }

  public Category? GetCategory(int id) {
    return repo.GetCategory(id);
  }

  public void UpdateCategory(Category category) {
    repo.UpdateCategory(category);
  }
}
