using System;

namespace StringToNumber
{
    public class StringToNumber
    {
        public int ConvertString(string sourceLine)
        {            
            var ordNumber=0;
            var resNumber=0;
            decimal tempNum=0;
            var negative=false;

            if (string.IsNullOrWhiteSpace(sourceLine)) throw new ArgumentException();
            
            if (sourceLine[0]=='-')
            {
                negative = true;
                sourceLine = sourceLine.Substring(1);
            }
            ordNumber = sourceLine.Length;
            foreach (var sign in sourceLine)
            {
                if (!char.IsDigit(sign)) throw new ArgumentOutOfRangeException();
                if (!negative)
                    tempNum += (decimal)((int)char.GetNumericValue(sign) * Math.Pow(10.0, ordNumber - 1));
                else
                    tempNum -= (decimal)((int)char.GetNumericValue(sign) * Math.Pow(10.0, ordNumber - 1));
                if (tempNum > int.MaxValue||tempNum < int.MinValue) throw new ArgumentOutOfRangeException();
                resNumber = (int)tempNum;
                ordNumber--;
            }
            return resNumber;
        }
    }
}
