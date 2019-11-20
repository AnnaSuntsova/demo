using NUnit.Framework;
using System;

namespace StringToNumberTests
{
    public class StringToNumberTests
    {
        [TestCase(" ", ExpectedResult = typeof(ArgumentException))]
        public int CheckEmptyString(string source)
        {
            var strToNum=new StringToNumber.StringToNumber();
            return strToNum.ConvertString(source);


           // Assert.Throws<ArgumentException>(()=> strToNum.ConvertString(" "));
        }

        [TestCase("123", ExpectedResult = 123)]
        [TestCase("-123", ExpectedResult = -123)]
        public int CheckCorrectString(string source)
        {
            var strToNum = new StringToNumber.StringToNumber();
            return strToNum.ConvertString(source);
        }

        [Test]
        public void CheckStringWithNumAndLetters()
        {
            var strToNum = new StringToNumber.StringToNumber();
            Assert.Throws<ArgumentOutOfRangeException>(() => strToNum.ConvertString("123!"));
        }

        [Test]
        public void CheckStringWithBiggerUlong()
        {
            var strToNum = new StringToNumber.StringToNumber();
            Assert.Throws<ArgumentOutOfRangeException>(() => strToNum.ConvertString("18446744073709551616"));
        }                                                                                       

    }

}