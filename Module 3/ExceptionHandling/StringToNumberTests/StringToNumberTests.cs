using NUnit.Framework;
using System;

namespace StringToNumberTests
{
    public class StringToNumberTests
    {
        [Test]
        public void CheckEmptyString()
        {
            var strToNum=new StringToNumber.StringToNumber();
            Assert.Throws<ArgumentException>(()=> strToNum.ConvertString(" "));
        }

        [TestCase("123", ExpectedResult = 123)]
        [TestCase("-123", ExpectedResult = -123)]
        [TestCase("0", ExpectedResult = 0)]
        [TestCase("2147483647", ExpectedResult = int.MaxValue)]
        [TestCase("-2147483648", ExpectedResult = int.MinValue)]
        public int CheckCorrectString(string source)
        {
            var strToNum = new StringToNumber.StringToNumber();
            return strToNum.ConvertString(source);
        }

        [TestCase("123!")]
        [TestCase("2147483648")]
        [TestCase("-2147483649")]
        public void CheckArgumentOutOfRange(string source)
        {
            var strToNum = new StringToNumber.StringToNumber();
            Assert.Throws<ArgumentOutOfRangeException>(() => strToNum.ConvertString(source));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void CheckArgumentExceptin(string source)
        {
            var strToNum = new StringToNumber.StringToNumber();
            Assert.Throws<ArgumentException>(() => strToNum.ConvertString(source));
        }

    }

}