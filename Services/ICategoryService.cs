using BusinessObjects;

namespace Services;
public interface ICategoryService {
  List<Category> GetCategories();
  Category? GetCategory(int id);
  void AddCategory(Category category);
  void DeleteCategory(Category category);
  void DeleteCategory(int id);
  void UpdateCategory(Category category);
}
