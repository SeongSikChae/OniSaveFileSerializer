using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.IO.Tests
{
    [TestClass]
    public class BinaryWriterExtensionsTests
    {
        [TestMethod]
        public void WriteKleiStringTest()
        {
            const string expected = "Hello, World";

            using MemoryStream memory = new MemoryStream();
            using BinaryReader reader = new BinaryReader(memory);
            using BinaryWriter writer = new BinaryWriter(memory);

            writer.WriteKleiString(expected);
            memory.Position = 0;

            string? actual = reader.ReadKleiString();
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void WriteKleiStringTest2()
        {
            using MemoryStream memory = new MemoryStream();
            using BinaryReader reader = new BinaryReader(memory);
            using BinaryWriter writer = new BinaryWriter(memory);

            writer.WriteKleiString(null);
            memory.Position = 0;

            string? actual = reader.ReadKleiString();
            Assert.IsNull(actual);
        }

        [TestMethod]
        public void WriteKleiStringTest3()
        {
            using MemoryStream memory = new MemoryStream();
            using BinaryReader reader = new BinaryReader(memory);
            using BinaryWriter writer = new BinaryWriter(memory);

            writer.WriteKleiString(string.Empty);
            memory.Position = 0;

            string? actual = reader.ReadKleiString();
            Assert.IsNotNull(actual);
            Assert.AreEqual(string.Empty, actual);
        }
    }
}