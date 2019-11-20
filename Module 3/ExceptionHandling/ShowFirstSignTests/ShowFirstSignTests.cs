using NUnit.Framework;
using System;

namespace ShowFirstSignTests
{
    public class ShowFirstSignTests
    {
        [TestCase("abc", ExpectedResult = 'a')]
        [TestCase(" cde", ExpectedResult = ' ')]
        public char ShowSignWithoutExc(string source)
        {
            var firstSign = new GetFirstSign.GetFirstSign();
            return firstSign.GetSign(source);
        }

        [Test]
        public void ShowSignOfEmptyLine()
        {
            var firstSign = new GetFirstSign.GetFirstSign();
            Assert.Throws<ArgumentException>(() => firstSign.GetSign(""));
        }
    }
}