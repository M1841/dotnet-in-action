namespace ComicSearchImproved;

public interface IXkcdClient
{
  Task<Comic> GetLatestAsync();
  Task<Comic?> GetByNumberAsync(int number);
}
