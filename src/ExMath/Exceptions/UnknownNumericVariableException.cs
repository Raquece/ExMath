using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExMath.Exceptions
{

    [Serializable]
    public class UnknownNumericVariableException : Exception
    {
        public UnknownNumericVariableException() { }
        public UnknownNumericVariableException(string message) : base(message) { }
        public UnknownNumericVariableException(string message, Exception inner) : base(message, inner) { }
        protected UnknownNumericVariableException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
