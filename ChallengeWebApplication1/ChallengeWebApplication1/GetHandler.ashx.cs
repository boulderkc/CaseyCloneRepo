using System;
using System.Web;

namespace ChallengeWebApplication1
{
    /// <summary>
    /// Summary description for GetHandler
    /// Note that Visual Studio 2017 no longer supports the Developer Server. The only choice was to use IIS Express or an external server. This web app uses IIS Express, 
    /// which may require you to open the solution, choose properties on the project, go to the web tab, and click "Create Virtual Directory".
    /// </summary>
    public class GetHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {

            // To force a 500 status, use a query like this: http://localhost:58523/GetHandler.ashx/?force500=true
            string force500 = context.Request.QueryString["force500"];
            if (force500 == "true")
            {
                context.Response.StatusDescription = "This is a forced 500 status based on the query string.";
                context.Response.StatusCode = 500; 
            }
            else
            {
                context.Response.ContentType = "application/json";
                context.Response.Write(DateTime.Now.ToLongTimeString());
            }

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}