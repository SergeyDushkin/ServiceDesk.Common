namespace servicedesk.Common.Host
{
    public interface IResolver
    {
        T Resolve<T>();
    }
}