namespace OniSaveFileSerializer.Serialize
{
    using Structure;

    public interface ISaveFileSerializer<T>
    {
        byte[] Serialize(T obj);

        void Serialize(BinaryWriter writer, T obj);

        T Deserialize(byte[] buf);

        T Deserialize(BinaryReader reader);
    }

    public sealed class NotInitializedException : Exception
    {
        public NotInitializedException(string message, Exception? innerException = null) : base(message, innerException) { }
    }
}
