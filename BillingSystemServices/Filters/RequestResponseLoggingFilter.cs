using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace BillingSystemServices.Filters
{
    public class RequestResponseLoggingFilter : ActionFilterAttribute
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(RequestResponseLoggingFilter));

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            LogRequest(actionContext.Request);
            base.OnActionExecuting(actionContext);
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception != null)
            {
                Log.Error("An error occurred during action execution:", actionExecutedContext.Exception);
            }
            else
            {
                LogResponse(actionExecutedContext.Response);
            }
            base.OnActionExecuted(actionExecutedContext);
        }


        private void LogRequest(HttpRequestMessage request)
        {
            Log.Info("Incoming request: " + request.RequestUri);
        }

        private void LogResponse(HttpResponseMessage response)
        {
            try
            {
                if (response != null)
                {
                    Log.Info("Outgoing response: " + response.RequestMessage.RequestUri + " - " + response.StatusCode);
                }
                else
                {
                    Log.Warn("Outgoing response is null.");
                }
            }
            catch (Exception ex)
            {
                Log.Error("An error occurred while logging the response:", ex);
                throw;
            }
        }
    }
}