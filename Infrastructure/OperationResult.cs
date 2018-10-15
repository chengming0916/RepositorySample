using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class OperationResult
    {
        private static readonly OperationResult _success = new OperationResult(true);

        public OperationResult(params string[] errors) : this((IEnumerable<string>)errors) { }

        public OperationResult(IEnumerable<string> errors)
        {
            if (errors == null) errors = new[] { "未知异常" };
            Succeeded = false;
            Errors = errors;
        }

        protected OperationResult(bool success)
        {
            Succeeded = success;
            Errors = new string[0];
        }

        public bool Succeeded { get; private set; }

        public IEnumerable<string> Errors { get; private set; }

        public static OperationResult Success
        {
            get { return _success; }
        }

        public static OperationResult Failed(params string[] errors)
        {
            return new OperationResult(errors);
        }
    }
}
