namespace ZEngine.Unity.Core.Collections.GOPools
{
    public interface GOPool<T>
    {
        T Get();

        void Return(T item);
    }
}