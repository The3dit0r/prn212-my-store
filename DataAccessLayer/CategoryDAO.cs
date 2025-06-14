using BusinessObjects;

namespace DataAccessLayer;
public class CategoryDAO {
    public CategoryDAO() {

    }

    public static List<Category> GetCategories() {
        var list = new List<Category>();

        using var context = new DatabaseContext();
        list = context.Categories.ToList();

        return list;
    }
}
