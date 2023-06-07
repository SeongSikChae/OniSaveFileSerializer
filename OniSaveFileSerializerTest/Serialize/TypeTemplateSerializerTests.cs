using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Cryptography;

namespace OniSaveFileSerializer.Serialize.Tests
{
    using Structure;

    [TestClass]
    public class TypeTemplateSerializerTests
    {
    }

    [TestClass]
    public class TypeInfoSerializerTests
    {
        [TestMethod]
        public void DeserializeTest()
        {
            using FileStream fs = new FileStream("TestData/TypeTemplate/typeInfo.save", FileMode.Open, FileAccess.Read);
            using BinaryReader reader = new BinaryReader(fs);
            TypeInfoSerializer serializer = new TypeInfoSerializer();
            TypeInfo typeInfo = serializer.Deserialize(reader);

            Assert.AreEqual(SerializationTypeInfo.IS_GENERIC_TYPE, typeInfo.Info & SerializationTypeInfo.IS_GENERIC_TYPE);
            Assert.AreEqual(2, typeInfo.SubTypes.Count);
            Assert.AreEqual(SerializationTypeInfo.Array, typeInfo.SubTypes[1].Info);
        }

        [TestMethod]
        public void DeserializeTest2()
        {
            using FileStream fs = new FileStream("TestData/TypeTemplate/typeInfo2.save", FileMode.Open, FileAccess.Read);
            using BinaryReader reader = new BinaryReader(fs);
            TypeInfoSerializer serializer = new TypeInfoSerializer();
            TypeInfo typeInfo = serializer.Deserialize(reader);

            Assert.AreEqual(SerializationTypeInfo.IS_VALUE_TYPE, typeInfo.Info);
            Assert.AreEqual(0, typeInfo.SubTypes.Count);
            Assert.IsNotNull(typeInfo.TemplateName);
            Assert.AreEqual("ModInfo", typeInfo.TemplateName);
        }

        [TestMethod]
        public void SerializeTest()
        {
            byte[] expected;
            {
                using FileStream fs = new FileStream("TestData/TypeTemplate/typeInfo2.save", FileMode.Open, FileAccess.Read);
                using BinaryReader reader = new BinaryReader(fs);
                expected = reader.ReadBytes(12);
            }

            {
                TypeInfo typeInfo = new TypeInfo
                {
                    Info = SerializationTypeInfo.IS_VALUE_TYPE,
                    TemplateName = "ModInfo"
                };

                TypeInfoSerializer serializer = new TypeInfoSerializer();
                byte[] actual = serializer.Serialize(typeInfo);

                SHA256 sha = SHA256.Create();
                string expectedHash = Convert.ToBase64String(sha.ComputeHash(expected));
                string actualHash = Convert.ToBase64String(sha.ComputeHash(actual));
                Assert.AreEqual(expectedHash, actualHash);
            }
        }
    }
}