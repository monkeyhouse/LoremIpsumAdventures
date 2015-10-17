
using Microsoft.AspNet.Http;
using System;
using System.Globalization;



namespace Web.Util
{

    public static class HttpExtensions
    {


        /// <summary>
        /// Determines whether the specified HTTP request is an AJAX request.
        /// </summary>
        /// <cite>Sourced from http://stackoverflow.com/a/29283185/1778606 </cite>
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            if (request.Headers != null)
                return request.Headers["X-Requested-With"] == "XMLHttpRequest";
            return false;
        }

    }

}