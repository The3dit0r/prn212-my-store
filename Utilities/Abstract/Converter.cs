namespace Utilities.Abstract;
public abstract class DataConverter<T, K> {
  public abstract K Convert(T item);
  public abstract T Reverse(K item);

}
