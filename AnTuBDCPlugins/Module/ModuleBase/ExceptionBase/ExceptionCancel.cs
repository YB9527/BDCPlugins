using System;
using System.Runtime.Serialization;

namespace ModuleBase.ExceptionBase
{
    [Serializable]
    public class ExceptionCancel : Exception
    {
        public ExceptionCancel() : base("操作被用户取消。") { }
        public ExceptionCancel(string message) : base(message) { }
        public ExceptionCancel(string message, Exception inner) : base(message, inner) { }
        protected ExceptionCancel(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
