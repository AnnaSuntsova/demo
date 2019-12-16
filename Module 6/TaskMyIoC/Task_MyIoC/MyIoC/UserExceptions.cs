using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyIoC
{
    public class ConstructorNotFoundException : Exception
    {
        public ConstructorNotFoundException(string message) : base(message)
        { }
    }

    public class NoConstructorAttributes :Exception
    {
        public NoConstructorAttributes(string message) : base(message)
        { }
    }
}
