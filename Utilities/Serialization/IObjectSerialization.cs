namespace Utilities
{
    public interface IObjectSerialization<T> where T : class
    {
        byte[] Serialize( T objectGraph );

        T DeSerialize( byte[] data );
    }
}
