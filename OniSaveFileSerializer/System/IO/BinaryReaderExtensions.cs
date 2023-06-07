namespace System.IO
{
    using Text;

    public static class BinaryReaderExtensions
    {
        public static string? ReadKleiString(this BinaryReader reader)
        {
            int count = reader.ReadInt32();
            if (count < -1)
                throw new IOKleiStringException($"Invalid byte count in ReadKleiString: {count}");
            if (count == -1)
                return null;
            if (count == 0)
                return string.Empty;
            byte[] buf = reader.ReadBytes(count);
            return Encoding.UTF8.GetString(buf);
        }
    }

    public sealed class IOKleiStringException : IOException
    {
        public IOKleiStringException(string message, Exception? innerException = null) : base(message, innerException) { }
    }
}
