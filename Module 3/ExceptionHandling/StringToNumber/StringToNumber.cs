using System;

namespace StringToNumber
{
    public class StringToNumber
    {
        public int ConvertString(string sourceLine)
        {
            sourceLine = sourceLine.Trim();
            var ordNumber = sourceLine.Length;
            var resNumber=0;
            decimal tempNum=0;
            var negative=false;

            if (string.IsNullOrEmpty(sourceLine)) throw new ArgumentException();
            for (var ind=0; ind<sourceLine.Length; ind++)
            {
                if (ind==0 && sourceLine[ind]=='-')
                {
                    negative = true;
                    ordNumber--;
                    continue;
                }
                if (!char.IsDigit(sourceLine[ind])) throw new ArgumentOutOfRangeException();
                tempNum += (decimal)((int)char.GetNumericValue(sourceLine[ind]) * Math.Pow(10.0, ordNumber - 1));
                if (tempNum >= int.MaxValue) throw new ArgumentOutOfRangeException();
                resNumber = (int)tempNum;
                ordNumber--;
            }

            if (negative)
                resNumber *= (-1);
            return resNumber;
        }
    }
}
