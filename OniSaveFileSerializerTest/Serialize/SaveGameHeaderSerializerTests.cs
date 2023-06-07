using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Cryptography;

namespace OniSaveFileSerializer.Serialize.Tests
{
    using Structure;

    [TestClass]
    public class SaveGameHeaderSerializerTests
    {
        [TestMethod]
        public void DeserializeTest()
        {
            using FileStream fs = new FileStream("TestData/Header/headerV7.31.save", FileMode.Open, FileAccess.Read);
            using BinaryReader reader = new BinaryReader(fs);
            SaveGameHeaderSerializer serializer = new SaveGameHeaderSerializer();
            SaveGameHeader header = serializer.Deserialize(reader);
            Assert.IsNotNull(header.Info);
            Assert.AreEqual<uint>(7, header.Info.SaveMajorVersion);
            Assert.AreEqual<uint>(31, header.Info.SaveMinorVersion);
        }

        [TestMethod]
        public void SerializeTest()
        {
            byte[] expectedBlock;
            {
                using FileStream fs = new FileStream("TestData/Header/headerV7.31.save", FileMode.Open, FileAccess.Read);
                using MemoryStream memory = new MemoryStream();
                fs.CopyTo(memory);
                expectedBlock = memory.ToArray();
            }

            SHA256 sha = SHA256.Create();
            string expectedHash = Convert.ToBase64String(sha.ComputeHash(expectedBlock));

            SaveGameHeaderSerializer serializer = new SaveGameHeaderSerializer();
            SaveGameHeader header = serializer.Deserialize(expectedBlock);
            byte[] actualBlock = serializer.Serialize(header);
            string actualHash = Convert.ToBase64String(sha.ComputeHash(actualBlock));

            Assert.AreEqual(expectedHash, actualHash);
        }
    }
}