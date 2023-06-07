namespace System
{
    using Text.RegularExpressions;

    public static class StringExtensions
    {
        public static void ValidateDotNetIdentifierName(this string? name)
        {
            if (string.IsNullOrEmpty(name))
                throw new NullOrEmptyIdentifierNameException("A .NET identifier name must not be null or zero length");
            if (name.Length >= 512)
                throw new MaxLengthIdentifierNameException("A .NET identifier name exceeded 511 characters. This most likely indicates a parser error.");
            if (IDENTIFIER_INVAL_CHARS.IsMatch(name))
                throw new NonPrintalbeIdentifierNameException("A .NET identifier name contains non-printable characters. This most likely indicates a parser error.");
        }

        private static readonly Regex IDENTIFIER_INVAL_CHARS = new Regex("[\x00-\x1F]");
    }

    public sealed class NullOrEmptyIdentifierNameException : Exception
    {
        public NullOrEmptyIdentifierNameException(string message, Exception? innerException = null) : base(message, innerException) { }
    }

    public sealed class MaxLengthIdentifierNameException : Exception
    {
        public MaxLengthIdentifierNameException(string message, Exception? innerException = null) : base(message, innerException) { }
    }

    public sealed class NonPrintalbeIdentifierNameException : Exception
    {
        public NonPrintalbeIdentifierNameException(string message, Exception? innerException = null) : base(message, innerException) { }
    }
}
