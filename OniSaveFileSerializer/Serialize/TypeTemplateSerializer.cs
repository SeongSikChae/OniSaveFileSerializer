namespace OniSaveFileSerializer.Serialize
{
    using Util;
    using Structure;

    public sealed class TypeTemplatesSerializer : ISaveFileSerializer<TypeTemplates>
    {
        public void Initialize(ISaveFileSerializer<TypeTemplate> typeTemplateSerializer)
        {
            this.typeTemplateSerializer = typeTemplateSerializer;
            this.initialized = true;
        }

        private ISaveFileSerializer<TypeTemplate>? typeTemplateSerializer;
        private bool initialized;

        public TypeTemplates Deserialize(byte[] buf)
        {
            using MemoryStream memory = new MemoryStream(buf);
            using BinaryReader reader = new BinaryReader(memory);
            return Deserialize(reader);
        }

        public TypeTemplates Deserialize(BinaryReader reader)
        {
            if (!initialized)
                throw new NotInitializedException("require serializer initialize");
            if (typeTemplateSerializer is null)
                throw new NullReferenceException("typeTemplateSerializer is null");
            int count = reader.ReadInt32();
            List<TypeTemplate> list = new List<TypeTemplate>();
            for (int i = 0; i < count; i++)
                list.Add(typeTemplateSerializer.Deserialize(reader));
            return new TypeTemplates
            {
                Items = list
            };
        }

        public byte[] Serialize(TypeTemplates obj)
        {
            using MemoryStream memory = new MemoryStream();
            using BinaryWriter writer = new BinaryWriter(memory);
            Serialize(writer, obj);
            return memory.ToArray();
        }

        public void Serialize(BinaryWriter writer, TypeTemplates obj)
        {
            if (!initialized)
                throw new NotInitializedException("require serializer initialize");
            if (typeTemplateSerializer is null)
                throw new NullReferenceException("typeTemplateSerializer is null");
            writer.Write(obj.Items.Count);
            foreach(TypeTemplate item in obj.Items)
                typeTemplateSerializer.Serialize(writer, item);
        }
    }

    public sealed class TypeTemplateSerializer : ISaveFileSerializer<TypeTemplate>
    {
        public void Initialize(ISaveFileSerializer<TypeTemplateMember> typeTemplateMemberSerializer)
        {
            this.typeTemplateMemberSerializer = typeTemplateMemberSerializer;
            this.initialized = true;
        }

        private bool initialized = false;
        private ISaveFileSerializer<TypeTemplateMember>? typeTemplateMemberSerializer;

        public TypeTemplate Deserialize(byte[] buf)
        {
            using MemoryStream memory = new MemoryStream(buf);
            using BinaryReader reader = new BinaryReader(memory);
            return Deserialize(reader);
        }

        public TypeTemplate Deserialize(BinaryReader reader)
        {
            if (!initialized)
                throw new NotInitializedException("require serializer initialize");
            if (typeTemplateMemberSerializer is null)
                throw new NullReferenceException("typeTemplateMemberSerializer is null");
            string? name = reader.ReadKleiString();
            name.ValidateDotNetIdentifierName();

            int fieldCount = reader.ReadInt32();
            int propCount = reader.ReadInt32();

            List<TypeTemplateMember> fieldList = new List<TypeTemplateMember>();
            for (int i = 0; i < fieldCount; i++)
            {
                TypeTemplateMember member = typeTemplateMemberSerializer.Deserialize(reader);
                fieldList.Add(member);
            }

            List<TypeTemplateMember> propList = new List<TypeTemplateMember>();
            for(int i = 0; i < propCount; i++)
            {
                TypeTemplateMember member = typeTemplateMemberSerializer.Deserialize(reader);
                propList.Add(member);
            }

            return new TypeTemplate
            {
                Name = name,
                Fields = fieldList,
                Props = propList
            };
        }

        public byte[] Serialize(TypeTemplate obj)
        {
            using MemoryStream memory = new MemoryStream();
            using BinaryWriter writer = new BinaryWriter(memory);
            Serialize(writer, obj);
            return memory.ToArray();
        }

        public void Serialize(BinaryWriter writer, TypeTemplate obj)
        {
            if (!initialized)
                throw new NotInitializedException("require serializer initialize");
            if (typeTemplateMemberSerializer is null)
                throw new NullReferenceException("typeTemplateMemberSerializer is null");
            writer.WriteKleiString(obj.Name);
            writer.Write(obj.Fields.Count);
            writer.Write(obj.Props.Count);
            foreach (TypeTemplateMember member in obj.Fields)
                typeTemplateMemberSerializer.Serialize(writer, member);
            foreach (TypeTemplateMember member in obj.Props)
                typeTemplateMemberSerializer.Serialize(writer, member);
        }
    }

    public sealed class TypeTemplateMemberSerializer : ISaveFileSerializer<TypeTemplateMember>
    {
        public void Initialize(ISaveFileSerializer<TypeInfo> typeInfoSerializer)
        {
            this.typeInfoSerializer = typeInfoSerializer;
            this.initialized = true;
        }

        private bool initialized = false;
        private ISaveFileSerializer<TypeInfo>? typeInfoSerializer;

        public TypeTemplateMember Deserialize(byte[] buf)
        {
            using MemoryStream memory = new MemoryStream(buf);
            using BinaryReader reader = new BinaryReader(memory);
            return Deserialize(reader);
        }

        public TypeTemplateMember Deserialize(BinaryReader reader)
        {
            if (!initialized)
                throw new NotInitializedException("require serializer initialize");
            if (typeInfoSerializer is null)
                throw new NullReferenceException("typeInfoSerializer is null");
            string? name = reader.ReadKleiString();
            name.ValidateDotNetIdentifierName();
            TypeInfo typeInfo = typeInfoSerializer.Deserialize(reader);
            return new TypeTemplateMember
            {
                Name = name,
                Type = typeInfo,
            };
        }

        public byte[] Serialize(TypeTemplateMember obj)
        {
            using MemoryStream memory = new MemoryStream();
            using BinaryWriter writer = new BinaryWriter(memory);
            Serialize(writer, obj);
            return memory.ToArray();
        }

        public void Serialize(BinaryWriter writer, TypeTemplateMember obj)
        {
            if (!initialized)
                throw new NotInitializedException("require serializer initialize");
            if (typeInfoSerializer is null)
                throw new NullReferenceException("typeInfoSerializer is null");
            if (obj.Type is null)
                throw new NullReferenceException("TypeInfo is null");
            writer.WriteKleiString(obj.Name);
            typeInfoSerializer.Serialize(writer, obj.Type);
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
            SerializationTypeCode code = SerializationTypeUtil.GetTypeCode(info);

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
            SerializationTypeCode code = SerializationTypeUtil.GetTypeCode(obj.Info);
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
