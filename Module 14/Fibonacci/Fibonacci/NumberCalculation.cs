using System;
using System.Collections.Generic;
using System.Text;

namespace Fibonacci
{
    public class NumberCalculation
    {
        private readonly ICache _cache;

        public NumberCalculation(ICache cache)
        {
            _cache = cache;
        }

        public int Calculate(int index)
        {
            if (index == 1 || index == 2)
            {
                return 1;
            }
            else
            {
                var storedValue = _cache.Get(index);
                if (storedValue != default(int))
                {
                    return storedValue;
                }
                int calcResult = Calculate(index - 1) + Calculate(index - 2);
                _cache.Set(index, calcResult);
                return calcResult;
            }
        }
    }
}
