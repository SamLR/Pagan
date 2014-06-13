namespace Pagan.Registry
{
    public interface IControllerFactory
    {
        Controller<T> GetController<T>();
    }
}