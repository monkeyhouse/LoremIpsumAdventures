using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActionResults
{

    /* MVC JSON Execption Handling */

    public class MessageResult : JsonResult
    {
        public MessageResult(string message) : base(new { Message = message })
        {
            StatusCode = (int)System.Net.HttpStatusCode.OK;
        }

        public MessageResult(object value,
                             System.Net.HttpStatusCode status = System.Net.HttpStatusCode.OK) : base(value)
        {
            if (value.GetType().GetProperty("Message") == null)
                throw new ArgumentException("Value must have a message propety", nameof(value));

            this.StatusCode = (int)status;
        }
    }


}
