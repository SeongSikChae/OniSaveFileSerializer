namespace System.IO
{
    using Text;

    public static class BinaryWriterExtensions
    {
        public static void WriteKleiString(this BinaryWriter writer, string? value) 
        {
            if (value is null)
            {
                writer.Write(-1);
                return;
            }

            if (string.Empty.Equals(value))
            {
                writer.Write(0);
                return;
            }

            byte[] buf = Encoding.UTF8.GetBytes(value);
            writer.Write(buf.Length);
            writer.Write(buf);
        }
    }
}
