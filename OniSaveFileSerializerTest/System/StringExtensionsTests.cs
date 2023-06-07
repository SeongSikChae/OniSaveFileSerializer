using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Tests
{
    [TestClass]
    public class StringExtensionsTests
    {
        [TestMethod]
        public void ValidateDotNetIdentifierNameTest()
        {
            string? str = null;
            Assert.ThrowsException<NullOrEmptyIdentifierNameException>(() =>
            {
                str.ValidateDotNetIdentifierName();
            });

            char[] charBuf = new char[512];
            for (int i = 0; i < charBuf.Length; i++)
                charBuf[i] = 'A';
            str = new string(charBuf);
            Assert.ThrowsException<MaxLengthIdentifierNameException>(() =>
            {
                str.ValidateDotNetIdentifierName();
            });

            charBuf = new char[10];
            charBuf[0] = '\x00';
            for (int i = 1; i < charBuf.Length; i++)
                charBuf[i] = 'A';
            str = new string(charBuf);
            Assert.ThrowsException<NonPrintalbeIdentifierNameException>(() =>
            {
                str.ValidateDotNetIdentifierName();
            });
        }
    }
}