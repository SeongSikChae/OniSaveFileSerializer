namespace OniSaveFileSerializer.Structure
{
    public sealed class TypeTemplate : ISaveFile
    {
        public string? Name { get; set; } = string.Empty;

        public List<TypeTemplateMember> Fields { get; set; } = new List<TypeTemplateMember>();

        public List<TypeTemplateMember> Props { get; set; } = new List<TypeTemplateMember>();

        public static bool IsGenericType(SerializationTypeCode code)
        {
            switch (code)
            {
                case SerializationTypeCode.Pair:
                case SerializationTypeCode.Dictionary:
                case SerializationTypeCode.List:
                case SerializationTypeCode.HashSet:
                case SerializationTypeCode.UserDefined:
                case SerializationTypeCode.Queue:
                    return true;
                default:
                    return false;
            }
        }
    }

    public sealed class TypeTemplates : ISaveFile
    {
        public List<TypeTemplate> Items = new List<TypeTemplate>();
    }

    public sealed class TypeTemplateMember : ISaveFile
    {
        public string? Name { get; set; }

        public TypeInfo? Type { get; set; }
    }

    public sealed class TypeInfo : ISaveFile
    {
        public SerializationTypeInfo Info { get; set; }

        public string? TemplateName { get; set; }

        public List<TypeInfo> SubTypes { get; set; } = new List<TypeInfo>();
    }

    public enum SerializationTypeInfo : byte
    {
        UserDefined = 0,
        SByte = 1,
        Byte = 2,
        Boolean = 3,
        Int16 = 4,
        UInt16 = 5,
        Int32 = 6,
        UInt32 = 7,
        Int64 = 8,
        UInt64 = 9,
        Single = 10,
        Double = 11,
        String = 12,
        Enumeration = 13,
        Vector2I = 14,
        Vector2 = 15,
        Vector3 = 16,
        Array = 17,
        Pair = 18,
        Dictionary = 19,
        List = 20,
        HashSet = 21,
        Queue = 22,
        Colour = 23,
        VALUE_MASK = 63,
        IS_VALUE_TYPE = 64,
        IS_GENERIC_TYPE = 128
    }

    public enum SerializationTypeCode : byte
    {
        UserDefined = 0,
        SByte = 1,
        Byte = 2,
        Boolean = 3,
        Int16 = 4,
        UInt16 = 5,
        Int32 = 6,
        UInt32 = 7,
        Int64 = 8,
        UInt64 = 9,
        Single = 10,
        Double = 11,
        String = 12,
        Enumeration = 13,
        Vector2I = 14,
        Vector2 = 15,
        Vector3 = 16,
        Array = 17,
        Pair = 18,
        Dictionary = 19,
        List = 20,
        HashSet = 21,
        Queue = 22,
        Colour = 23
    }
}
