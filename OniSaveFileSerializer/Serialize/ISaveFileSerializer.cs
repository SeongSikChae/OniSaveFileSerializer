namespace OniSaveFileSerializer.Serialize
{
    using Structure;

    public interface ISaveFileSerializer<T> where T : ISaveFile
    {
        byte[] Serialize(T obj);

        void Serialize(BinaryWriter writer, T obj);

        T Deserialize(byte[] buf);

        T Deserialize(BinaryReader reader);
    }
}
