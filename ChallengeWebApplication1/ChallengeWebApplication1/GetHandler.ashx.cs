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
            // Want to prove that the StartTimeUTC and EndTimeUTC are working? Uncomment this .Sleep function to add a 2 second pause, which will make the start and end seconds different. 
            //System.Threading.Thread.Sleep(2000);

            // Want to see the -999 timeout errors in the client app? Add a very long sleep here, such as 120 seconds
            //System.Threading.Thread.Sleep(120000);


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