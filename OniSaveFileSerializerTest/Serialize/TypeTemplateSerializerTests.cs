using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Cryptography;

namespace OniSaveFileSerializer.Serialize.Tests
{
    using Structure;
    using System.Diagnostics.Metrics;

    [TestClass]
    public class TypeTemplateSerializerTests
    {
        [TestMethod]
        public void InitializeTest()
        {
            TypeTemplateSerializer serializer = new TypeTemplateSerializer();
            Assert.ThrowsException<NotInitializedException>(() =>
            {
                serializer.Serialize(new TypeTemplate
                {
                    Name = "Test"
                });
            });
        }

        [TestMethod]
        public void SerializeAndDeserializeTest()
        {
            TypeTemplateSerializer serializer = new TypeTemplateSerializer();
            serializer.Initialize(new DummyTypeTemplateMemberSerializer());
            byte[] block1 = serializer.Serialize(new TypeTemplate
            { 
                Name = "Test" 
            });

            TypeTemplate template = serializer.Deserialize(block1);
            Assert.IsNotNull(template.Name);
            Assert.AreEqual("Test", template.Name);
            Assert.AreEqual(0, template.Fields.Count);
            Assert.AreEqual(0, template.Props.Count);
        }

        private sealed class DummyTypeTemplateMemberSerializer : ISaveFileSerializer<TypeTemplateMember>
        {
            public TypeTemplateMember Deserialize(byte[] buf)
            {
                throw new NotImplementedException();
            }

            public TypeTemplateMember Deserialize(BinaryReader reader)
            {
                throw new NotImplementedException();
            }

            public byte[] Serialize(TypeTemplateMember obj)
            {
                throw new NotImplementedException();
            }

            public void Serialize(BinaryWriter writer, TypeTemplateMember obj)
            {
                throw new NotImplementedException();
            }
        }
    }

    [TestClass]
    public class TypeTemplateMemberSerializerTests
    {
        [TestMethod]
        public void InitializeTest()
        {
            TypeTemplateMemberSerializer serializer = new TypeTemplateMemberSerializer();
            Assert.ThrowsException<NotInitializedException>(() =>
            {
                serializer.Serialize(new TypeTemplateMember
                {
                    Name = "Test",
                    Type = new TypeInfo
                    {
                        Info = SerializationTypeInfo.IS_VALUE_TYPE,
                        TemplateName = "ModInfo"
                    }
                });
            });
        }

        [TestMethod]
        public void SerializeAndDeserializeTest()
        {
            TypeTemplateMemberSerializer serializer = new TypeTemplateMemberSerializer();
            serializer.Initialize(new DummyTypeInfoSerializer());
            byte[] block1 = serializer.Serialize(new TypeTemplateMember
            {
                Name = "Test",
                Type = new TypeInfo
                {
                    Info = SerializationTypeInfo.IS_VALUE_TYPE,
                    TemplateName = "ModInfo"
                }
            });

            TypeTemplateMember member = serializer.Deserialize(block1);
            Assert.AreEqual("Test", member.Name);
            Assert.IsNotNull(member.Type);
            Assert.AreEqual(SerializationTypeInfo.IS_VALUE_TYPE, member.Type.Info);
            Assert.AreEqual("ModInfo", member.Type.TemplateName);
        }

        private sealed class DummyTypeInfoSerializer : ISaveFileSerializer<TypeInfo>
        {
            public TypeInfo Deserialize(byte[] buf)
            {
                throw new NotImplementedException();
            }

            public TypeInfo Deserialize(BinaryReader reader)
            {
                return new TypeInfo
                {
                    Info = SerializationTypeInfo.IS_VALUE_TYPE,
                    TemplateName = "ModInfo"
                };
            }

            public byte[] Serialize(TypeInfo obj)
            {
                throw new NotImplementedException();
            }

            public void Serialize(BinaryWriter writer, TypeInfo obj)
            {
                writer.Write((byte)obj.Info);
                writer.WriteKleiString(obj.TemplateName);
            }
        }
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

        [TestMethod]
        public void SerializeTest2()
        {
            byte[] expected;
            {
                using FileStream fs = new FileStream("TestData/TypeTemplate/typeInfo.save", FileMode.Open, FileAccess.Read);
                expected = new byte[(int)fs.Length];
                fs.Read(expected, 0, expected.Length);
            }

            TypeInfoSerializer serializer = new TypeInfoSerializer();
            TypeInfo typeInfo = serializer.Deserialize(expected);

            byte[] actual = serializer.Serialize(typeInfo);
            SHA256 sha = SHA256.Create();
            string expectedHash = Convert.ToBase64String(sha.ComputeHash(expected));
            string actualHash = Convert.ToBase64String(sha.ComputeHash(actual));
            Assert.AreEqual(expectedHash, actualHash);
        }
    }
}