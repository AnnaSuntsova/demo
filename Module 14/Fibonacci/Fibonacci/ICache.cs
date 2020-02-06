using System;
using System.Collections.Generic;
using System.Text;

namespace Fibonacci
{
    public interface ICache
    {
        int Get(int key);
        void Set(int key, int value);
    }
}
