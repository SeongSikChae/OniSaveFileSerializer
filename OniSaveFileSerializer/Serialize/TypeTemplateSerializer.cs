namespace OniSaveFileSerializer.Serialize
{
    using Structure;

    public sealed class TypeTemplateSerializer : ISaveFileSerializer<TypeTemplate>
    {
        public TypeTemplate Deserialize(byte[] buf)
        {
            throw new NotImplementedException();
        }

        public TypeTemplate Deserialize(BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        public byte[] Serialize(TypeTemplate obj)
        {
            throw new NotImplementedException();
        }

        public void Serialize(BinaryWriter writer, TypeTemplate obj)
        {
            throw new NotImplementedException();
        }
    }

    public sealed class TypeTemplateMemberSerializer : ISaveFileSerializer<TypeTemplateMember>
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

    public sealed class TypeInfoSerializer : ISaveFileSerializer<TypeInfo>
    {
        public TypeInfo Deserialize(byte[] buf)
        {
            using MemoryStream memory = new MemoryStream(buf);
            using BinaryReader reader = new BinaryReader(memory);
            return Deserialize(reader);
        }

        public TypeInfo Deserialize(BinaryReader reader)
        {
            SerializationTypeInfo info = (SerializationTypeInfo) reader.ReadByte();
            SerializationTypeCode code = GetTypeCode(info);

            string templateName = string.Empty;
            if (code == SerializationTypeCode.UserDefined || code == SerializationTypeCode.Enumeration)
            {
                string? userTypeName = reader.ReadKleiString();
                if (userTypeName is null)
                    throw new NullReferenceException("Expected non-null type name for user-defined or enumeration type.");
                templateName = userTypeName;
            }

            List<TypeInfo> subTypeList = new List<TypeInfo>();
            if ((info & SerializationTypeInfo.IS_GENERIC_TYPE) == SerializationTypeInfo.IS_GENERIC_TYPE)
            {
                if (!TypeTemplate.IsGenericType(code))
                    throw new Exception($"Unsupported non-generic type {Enum.GetName(code)} marked as generic.");
                byte subTypeCount = reader.ReadByte();
                for (int i = 0; i < subTypeCount; i++)
                    subTypeList.Add(Deserialize(reader));
            }
            else if (code == SerializationTypeCode.Array)
                subTypeList.Add(Deserialize(reader));
            return new TypeInfo
            {
                Info = info,
                TemplateName = templateName,
                SubTypes = subTypeList
            };
        }

        private static SerializationTypeCode GetTypeCode(SerializationTypeInfo info)
        {
            return (SerializationTypeCode)(info & SerializationTypeInfo.VALUE_MASK);
        }

        public byte[] Serialize(TypeInfo obj)
        {
            using MemoryStream memory = new MemoryStream();
            using BinaryWriter writer = new BinaryWriter(memory);
            Serialize(writer, obj);
            return memory.ToArray();
        }

        public void Serialize(BinaryWriter writer, TypeInfo obj)
        {
            writer.Write((byte)obj.Info);
            SerializationTypeCode code = GetTypeCode(obj.Info);
            if (code == SerializationTypeCode.UserDefined || code == SerializationTypeCode.Enumeration)
                writer.WriteKleiString(obj.TemplateName);
            if ((obj.Info & SerializationTypeInfo.IS_GENERIC_TYPE) == SerializationTypeInfo.IS_GENERIC_TYPE)
            {
                if (!TypeTemplate.IsGenericType(code))
                    throw new Exception($"Unsupported non-generic type {Enum.GetName(code)} marked as generic.");
                writer.Write((byte)obj.SubTypes.Count);
                foreach (TypeInfo subType in obj.SubTypes)
                    Serialize(writer, subType);
            }
            else if (code == SerializationTypeCode.Array)
                Serialize(writer, obj.SubTypes[0]);
        }
    }
}
