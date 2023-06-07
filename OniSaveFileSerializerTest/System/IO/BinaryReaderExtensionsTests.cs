using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;

namespace System.IO.Tests
{
    [TestClass]
    public class BinaryReaderExtensionsTests
    {
        [TestMethod]
        public void ReadKleiStringTest()
        {
            using MemoryStream memory = new MemoryStream();
            using BinaryReader reader = new BinaryReader(memory);
            using BinaryWriter writer = new BinaryWriter(memory);

            byte[] block = Encoding.UTF8.GetBytes("Hello, World");
            writer.Write(block.Length);
            writer.Write(block, 0, block.Length);

            memory.Position = 0;

            string? kleiStr = reader.ReadKleiString();
            Assert.IsNotNull(kleiStr);
            Assert.AreEqual("Hello, World", kleiStr);
        }

        [TestMethod]
        public void ReadKleiStringTest2()
        {
            using MemoryStream memory = new MemoryStream();
            using BinaryReader reader = new BinaryReader(memory);
            using BinaryWriter writer = new BinaryWriter(memory);

            writer.Write(-2);
            memory.Position = 0;

            Assert.ThrowsException<IOKleiStringException>(() =>
            {
                reader.ReadKleiString();
            });
        }

        [TestMethod]
        public void ReadKleiStringTest3()
        {
            using MemoryStream memory = new MemoryStream();
            using BinaryReader reader = new BinaryReader(memory);
            using BinaryWriter writer = new BinaryWriter(memory);

            writer.Write(-1);
            memory.Position = 0;

            string? kleiStr = reader.ReadKleiString();
            Assert.IsNull(kleiStr);
        }

        [TestMethod]
        public void ReadKleiStringTest4()
        {
            using MemoryStream memory = new MemoryStream();
            using BinaryReader reader = new BinaryReader(memory);
            using BinaryWriter writer = new BinaryWriter(memory);

            writer.Write(0);
            memory.Position = 0;

            string? kleiStr = reader.ReadKleiString();
            Assert.IsNotNull(kleiStr);
            Assert.AreEqual(string.Empty, kleiStr);
        }
    }
}