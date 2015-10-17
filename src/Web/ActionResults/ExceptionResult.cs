using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActionResults
{

    /* JSON Execption Handling */

    public class ExceptionMessage : ApplicationException
    {
        public JsonResult exceptionDetails;
        public ExceptionMessage(JsonResult exceptionDetails)
        {
            this.exceptionDetails = exceptionDetails;
        }
        public ExceptionMessage(string message) : base(message) { }
        public ExceptionMessage(string message, Exception inner) : base(message, inner) { }
        protected ExceptionMessage(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        { }
    }

    public class HandleUIExceptionAttribute : ActionFilterAttribute, IExceptionFilter
    {
        public virtual void OnException(ExceptionContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }
            if (filterContext.Exception != null)
            {
                filterContext.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;

                var message = filterContext.Exception;
                var result = new JsonResult(message);
                filterContext.Result = new ExceptionMessage(result).exceptionDetails;
            }
        }
    }

}
