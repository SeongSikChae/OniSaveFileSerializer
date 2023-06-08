using OniSaveFileSerializer.Structure;

namespace OniSaveFileSerializer.Util
{
    using Structure;

    internal static class SerializationTypeUtil
    {
        public static SerializationTypeCode GetTypeCode(SerializationTypeInfo info)
        {
            return (SerializationTypeCode)(info & SerializationTypeInfo.VALUE_MASK);
        }
    }
}
