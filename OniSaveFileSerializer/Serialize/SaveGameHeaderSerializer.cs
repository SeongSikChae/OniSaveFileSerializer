using System.IO;
using System.Text;
using System.Text.Json;

namespace OniSaveFileSerializer.Serialize
{
    using Structure;

    public sealed class SaveGameHeaderSerializer : ISaveFileSerializer<SaveGameHeader>
    {
        public SaveGameHeader Deserialize(byte[] buf)
        {
            using MemoryStream memory = new MemoryStream(buf);
            using BinaryReader reader = new BinaryReader(memory);
            return Deserialize(reader);
        }

        public SaveGameHeader Deserialize(BinaryReader reader)
        {
            SaveGameHeader header = new SaveGameHeader();
            header.BuildVersion = reader.ReadUInt32();
            header.InfoSize = reader.ReadUInt32();
            header.HeaderVersion = reader.ReadUInt32();
            header.Compressed = reader.ReadUInt32() == 1;

            byte[] infoBuf = reader.ReadBytes((int)header.InfoSize);
            string infoJson = Encoding.UTF8.GetString(infoBuf);
            header.Info = JsonSerializer.Deserialize<SaveGameHeader.SaveGameInfo>(infoJson);
            if (header.Info is null)
                throw new NullReferenceException("SaveGameHeader Info is null");
            return header;
        }

        public byte[] Serialize(SaveGameHeader obj)
        {
            using MemoryStream memory = new MemoryStream();
            using BinaryWriter writer = new BinaryWriter(memory);
            Serialize(writer, obj);
            return memory.ToArray();
        }

        public void Serialize(BinaryWriter writer, SaveGameHeader obj)
        {
            if (obj.Info is null)
                throw new NullReferenceException("SaveGameHeader Info is null");
            string infoJson = JsonSerializer.Serialize(obj.Info);
            byte[] infoBuf = Encoding.UTF8.GetBytes(infoJson);
            obj.InfoSize = (uint)infoBuf.Length;
            writer.Write(obj.BuildVersion);
            writer.Write(obj.InfoSize);
            writer.Write(obj.HeaderVersion);
            uint compressed = (uint)(obj.Compressed ? 1 : 0);
            writer.Write(compressed);
            writer.Write(infoBuf);
        }
    }
}
