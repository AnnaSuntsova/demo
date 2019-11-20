using System;

namespace GetFirstSign
{
    public class GetFirstSign
    {
        public char GetSign(string sourceStr)
        {
            if (string.IsNullOrEmpty(sourceStr)) throw new ArgumentException();
            return sourceStr[0];
        }
    }
}
