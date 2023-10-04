using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSuite.Definitions
{
    internal struct InvalidStruct : IEquatable<InvalidStruct>
    {
        public bool Equals(InvalidStruct other)
        {
            return true;
        }
    }
}
