namespace Pagan.Registry
{
    public interface ITableFactory
    {
        Table<T> GetTable<T>();
    }
}