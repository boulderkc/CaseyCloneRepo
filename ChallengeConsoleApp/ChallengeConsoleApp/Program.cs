using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;


namespace ChallengeConsoleApp
{
    class Program
    {
        static string _theRetString;

        static void Main()
        {
            
            List<int> intList = new List<int> { 1, 2, 3, 4, 5 };
            // Sum the ints
            Console.WriteLine($"Sum of ints: {SumListOfInts(intList).ToString()}");
            // Sum the even ints
            Console.WriteLine($"Sum of even ints: {SumEvenNumbers(intList).ToString()}");


            // Get thing from HTTP client, place it in _theRetString
            RunAsync().GetAwaiter().GetResult();
            //await RunAsync();
            Console.WriteLine($"DateTime from HTTP get: {_theRetString}");

            // The thread delay settings for the two threads are in app.config, where they can be configured. 
            //Console.WriteLine($"Thread delay 1 setting: {StaticUtilities.GetThreadDelay1Setting()}");
            //Console.WriteLine($"Thread delay 2 setting: {StaticUtilities.GetThreadDelay2Setting()}");
            RunTwoThreadsWithDelay(intList);
 

            // pause for user input
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }

        public static void RunTwoThreadsWithDelay(List<int> intList)
        {

            //Console.WriteLine($"Thread delay 1 setting: {StaticUtilities.GetThreadDelay1Setting()}");
            //Console.WriteLine($"Thread delay 2 setting: {StaticUtilities.GetThreadDelay2Setting()}");


            // Create an array of Thread references.
            Thread[] array = new Thread[2];
            // New each thread with ParameterizedThreadStart. This will allow us to call the Start with an object (a struct, in this case), which we'll use to pass 
            // our list of integers and our delay.
            ParameterizedThreadStart start = new ParameterizedThreadStart(StaticUtilities.Write1);
            array[0] = new Thread(start);
            StaticUtilities.ListOfIntsAndDelayStruct firstThreadValues = new StaticUtilities.ListOfIntsAndDelayStruct(intList, StaticUtilities.GetThreadDelay1Setting());
            array[0].Start(firstThreadValues);


            array[1] = new Thread(start);
            StaticUtilities.ListOfIntsAndDelayStruct secondThreadValues = new StaticUtilities.ListOfIntsAndDelayStruct(intList, StaticUtilities.GetThreadDelay2Setting());
            array[1].Start(secondThreadValues);

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

 
        static async Task RunAsync()
        {
            HttpClient client = new HttpClient();
            // Update port # in the following line.
            client.BaseAddress = new Uri("http://ipv6.fiddler:59383/");


            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                // Get the XXX TODO
                _theRetString = await GetAllSessionsResultAsync("http://localhost:59383/api/allsessionslaunch", client);
                Console.WriteLine(_theRetString);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        // In order to make ReadAsAsync work, needed to get nuget AspNet.WebApi.Client package, "install-package Microsoft.AspNet.WebApi.Client"
        static async Task<string> GetAllSessionsResultAsync(string path, HttpClient client)
        {
            _theRetString = "";
            var response = await client.GetAsync(path, HttpCompletionOption.ResponseHeadersRead); 


            if (response.IsSuccessStatusCode)
            {
                _theRetString = await response.Content.ReadAsAsync<string>();
            }
            return _theRetString;
        }

    }
}
