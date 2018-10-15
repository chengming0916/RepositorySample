using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Exceptions
{
    /// <summary>
    /// 异常消息封装
    /// </summary>
    public class ExceptionMessage
    {
        /// <summary>
        /// 自定义异常信息
        /// </summary>
        /// <param name="ex">异常对象</param>
        /// <param name="userMessage">自定义消息</param>
        /// <param name="isHideStackTrace">是否跟踪异常堆栈信息</param>
        public ExceptionMessage(Exception ex, string userMessage = null, bool isHideStackTrace = false)
        {
            UserMessage = string.IsNullOrEmpty(userMessage) ? ex.Message : userMessage;

            StringBuilder builder = new StringBuilder();
            Message = string.Empty;
            int count = 0;
            string appString = "";
            while (ex != null)
            {
                if (count > 0)
                    appString += "　";
                Message = ex.Message;
                builder.AppendLine(appString + "异常消息：" + ex.Message);
                builder.AppendLine(appString + "异常类型：" + ex.GetType().FullName);
                builder.AppendLine(appString + "异常方法：" + (ex.TargetSite?.Name));
                builder.AppendLine(appString + "异常源：" + ex.Source);
                if (!isHideStackTrace && ex.StackTrace != null)
                    builder.AppendLine(appString + "异常堆栈：" + ex.StackTrace);
                if (ex.InnerException != null)
                {
                    builder.AppendLine(appString + "内部异常：");
                    count++;
                }
                ex = ex.InnerException;
            }
            ErrorDetails = builder.ToString();
        }

        /// <summary>
        /// 异常描述，包含异常消息，规模信息，异常类型，异常源，引发异常的方法及内部消息
        /// </summary>
        public string ErrorDetails { get; set; }

        /// <summary>
        /// Exception异常消息，即ex.Message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 报告给用户的异常消息
        /// </summary>
        public string UserMessage { get; set; }
    }
}
