using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;


namespace ChallengeConsoleApp
{
    // WebReturnValues holds values related to the HTTP get
    class WebReturnValues
    {
        public int httpStatusCode { get; set; }
        public int internalStatusCode { get; set; }
        public string httpResponseValue { get; set; }
        public string requestSentDateTimeUTC { get; set; }
        public string responseReceivedDateTimeUTC { get; set; }
        public string statusString { get; set; }

        // We need a constructor with no params because this allows us to use object initialization rather than using "new"
        public WebReturnValues()
        {
        }
    }

    class Program
    {
        static WebReturnValues _theReturnValues; 

        static void Main()
        {        
            List<int> intList = new List<int> { 1, 2, 3, 4, 5 };

            // Sum the ints
            Console.WriteLine($"Sum of ints: {SumListOfInts(intList).ToString()}");
            // Sum the even ints
            Console.WriteLine($"Sum of even ints: {SumEvenNumbers(intList).ToString()}");

            // Get values from HTTP client, place them in _theReturnValues
            RunAsync().GetAwaiter().GetResult();
            Console.WriteLine($"Time retrieved from HTTP GetHandler: {_theReturnValues.httpResponseValue}");

            // Write to SQL log 
            SQLStaticUtilities.AddLog(Convert.ToDateTime(_theReturnValues.requestSentDateTimeUTC), Convert.ToDateTime(_theReturnValues.responseReceivedDateTimeUTC), _theReturnValues.httpStatusCode, _theReturnValues.httpResponseValue, _theReturnValues.internalStatusCode, _theReturnValues.statusString); 

            // The thread delay settings for the two threads are in app.config, where they can be configured manually. 
            //Console.WriteLine($"Thread delay 1 setting: {StaticUtilities.GetThreadDelay1Setting()}");
            //Console.WriteLine($"Thread delay 2 setting: {StaticUtilities.GetThreadDelay2Setting()}");
            StaticUtilities.RunTwoThreadsWithDelay(intList);


            // pause for user input
            Console.WriteLine("\nPress any key to continue");
            Console.ReadKey();
        }


        public static int SumListOfInts(List<int> ints)
        {
            int ret = ints.Sum();
            return ret;
        }

        public static int SumEvenNumbers(List<int> ints)
        {
            int ret = ints.Where(s => s % 2 == 0).Sum(); 
                return ret;
        }



        #region HTTP async functions

        static async Task RunAsync()
        {
            HttpClient client = new HttpClient();
            // This app uses IIS Express. It requires a localhost path and port. The base path can be configured in the application configuration (app.config in the 
            // project, or ChallengeConsoleApp.exe.config in the binaries folder) file. You may need to generate a virtual director for IIS first. To do this, open the
            // web applicaton project, go to the project properties, go to the web tab, in the Servers section click "Create Virtual Directory". This will populate
            // the Project URL with something like "http://localhost:58523/", which is the value you should paste into the app.config.
            client.BaseAddress = new Uri(StaticUtilities.GetLocalhostBaseAddressSetting());


            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {
                // Get the time from web app

                // Case 1: normal, successful 200 response, should return the DateTime.
                _theReturnValues = await GetTimeResultAsync(StaticUtilities.GetLocalhostBaseAddressSetting() + "GetHandler.ashx", client);

                // Case 2: force a 500 response. To see this occur, comment out Case 1, uncomment this next line. 
                //_theReturnValues = await GetTimeResultAsync(StaticUtilities.GetLocalhostBaseAddressSetting() + "GetHandler.ashx/?force500=true", client);

                // Case 3: timeout, or other exception error. To cause a real timeout, modify the "ProcessRequest" function in ChallengeWebApplication1 to include a very 
                // long sleep timer. 


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }


        // In order to make ReadAsAsync work, needed to get nuget AspNet.WebApi.Client package, "install-package Microsoft.AspNet.WebApi.Client"
        static async Task<WebReturnValues> GetTimeResultAsync(string path, HttpClient client)
        {
            // construct a default WebReturnValues
            _theReturnValues = new WebReturnValues { httpResponseValue = "", httpStatusCode = 500, internalStatusCode = 2, requestSentDateTimeUTC = "", responseReceivedDateTimeUTC = "" };

            try
            {
                _theReturnValues.requestSentDateTimeUTC = DateTime.UtcNow.ToString();
                var response = await client.GetAsync(path, HttpCompletionOption.ResponseHeadersRead);
                _theReturnValues.responseReceivedDateTimeUTC = DateTime.UtcNow.ToString(); // Since we used "await" for the GetAsync call, now should be the response datetime.

                if (response.IsSuccessStatusCode)
                {
                    _theReturnValues.httpResponseValue = await response.Content.ReadAsStringAsync();
                    _theReturnValues.httpStatusCode = 200;
                    _theReturnValues.internalStatusCode = 1;
                    _theReturnValues.statusString = "Success";
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError) // Web app returned a 500 status code, probably because we forced it to with query string!
                {

                    _theReturnValues.httpResponseValue = await response.Content.ReadAsStringAsync();
                    _theReturnValues.httpStatusCode = 500;
                    _theReturnValues.internalStatusCode = 2;
                    _theReturnValues.statusString = "Web app returned a 500 status code, probably because we forced it to with query string.";
                }
                //else if (response.StatusCode == System.Net.HttpStatusCode.GatewayTimeout || response.StatusCode == System.Net.HttpStatusCode.RequestTimeout) // These aren't server timeouts, they are not useful for this application.
                //{
                //    _theReturnValues.httpResponseValue = await response.Content.ReadAsStringAsync();
                //    _theReturnValues.httpStatusCode = 408;
                //    _theReturnValues.internalStatusCode = -999;
                //}
                else
                {
                    _theReturnValues.httpResponseValue = await response.Content.ReadAsStringAsync();
                    _theReturnValues.httpStatusCode = 500;
                    _theReturnValues.internalStatusCode = 2;
                    _theReturnValues.statusString = "Failed to get time from web app, unknown reason.";
                }

            }
            catch (Exception e)  // This exception will be cause either because our URL is bad, the web app is not running, or the server timed out. We're going to treat it as a timeout, mostly.
            {
                _theReturnValues.httpResponseValue = $"Exception caught. Message: {e.Message}";
                _theReturnValues.httpStatusCode = 500;
                _theReturnValues.internalStatusCode = -999; // Maybe a timeout.
                _theReturnValues.responseReceivedDateTimeUTC = DateTime.UtcNow.ToString();
                _theReturnValues.statusString = "This could either mean the HTTP response timed out, or it was an invalid request.";
                return _theReturnValues;
            }

            return _theReturnValues;
        }

        #endregion HTTP async functions
    }
}
