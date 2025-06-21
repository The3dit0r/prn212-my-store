
using Microsoft.EntityFrameworkCore;
namespace DataAccessLayer;

public class _BaseDAO<T> where T : class {
  protected readonly DbSet<T> _dbSet;

  public _BaseDAO(DbSet<T> dbSet) {
    _dbSet = dbSet;
  }

  public List<T> GetAllItems() {
    List<T> items = [.. _dbSet.ToList()];
    return items;
  }

  public List<T> GetItems(Func<T, bool> condition) {
    return GetAllItems().Where(condition).ToList();
  }

  public T? GetItem(Func<T, bool> condition) {
    return GetItems(condition).FirstOrDefault();
  }

  public void AddItem(T item) {
     _dbSet.Add(item);
  }

  public void UpdateItem(T item) {
    _dbSet.Update(item);
  }

  public void RemoveItems(Func<T, bool> condition) {
    var list = GetItems(condition);
    foreach (var item in list) {
      RemoveItem(item);
    }
  }

  public void RemoveItem(T item) {
    _dbSet.Remove(item);
  }
}