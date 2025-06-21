
using BusinessObjects;
using Microsoft.EntityFrameworkCore;
namespace DataAccessLayer;

public class _BaseDAO<T> where T : class {
  protected readonly DbSet<T> _dbSet;
  protected readonly DatabaseContext context;

  public _BaseDAO(DatabaseContext c, string db) {
    context = c;
    _dbSet = (DbSet<T>)c.GetType().GetProperty(db).GetValue(c, null);

    if (_dbSet == null) {
      throw new Exception($"Property {db} does not exist on current database context");
    }
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
    context.SaveChanges();
  }

  public void UpdateItem(T item) {
    _dbSet.Update(item);
    context.SaveChanges();
  }

  public void RemoveItems(Func<T, bool> condition) {
    var list = GetItems(condition);
    foreach (var item in list) {
      RemoveItem(item, false);
    }

    context.SaveChanges();
  }

  void RemoveItem(T item, bool save) {
    _dbSet.Remove(item);

    if (save) {
      context.SaveChanges();
    }
  }

  public void RemoveItem(T item) {
    RemoveItem(item, true);
  }
}