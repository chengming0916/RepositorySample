using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Exceptions
{
    /// <summary>
    /// 组件异常封装
    /// </summary>
    [Serializable]
    public class ComponentException : Exception
    {
        public ComponentException() { }

        public ComponentException(string message)
            : base(message) { }

        public ComponentException(string message, Exception innerException)
            : base(message, innerException) { }

        public ComponentException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}
