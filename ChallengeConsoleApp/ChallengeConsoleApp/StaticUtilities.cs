using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChallengeConsoleApp
{
    public class StaticUtilities
    {
        public struct ListOfIntsAndDelayStruct
        {
            public List<int> listOfInts { get; set; }
            public int delayMilliseconds { get; set; }

            public ListOfIntsAndDelayStruct(List<int> prop1, int prop2)
            {
                listOfInts = prop1;
                delayMilliseconds = prop2; 
            }
        }

        public static void RunTwoThreadsWithDelay(List<int> intList)
        {
            // Create an array of Thread references.
            Thread[] array = new Thread[2];
            // New each thread with ParameterizedThreadStart. This will allow us to call the Start with an object (a struct, in this case), which we'll use to pass 
            // our list of integers and our delay.
            ParameterizedThreadStart start = new ParameterizedThreadStart(StaticUtilities.WriteThreadDelegate);
            // Thread 1
            array[0] = new Thread(start);
            array[0].Name = "T1";
            StaticUtilities.ListOfIntsAndDelayStruct firstThreadValues = new StaticUtilities.ListOfIntsAndDelayStruct(intList, StaticUtilities.GetThreadDelay1Setting());
            array[0].Start(firstThreadValues);

            // Thread 2
            array[1] = new Thread(start);
            array[1].Name = "T2";
            StaticUtilities.ListOfIntsAndDelayStruct secondThreadValues = new StaticUtilities.ListOfIntsAndDelayStruct(intList, StaticUtilities.GetThreadDelay2Setting());
            array[1].Start(secondThreadValues);

            while (array[0].IsAlive || array[1].IsAlive)
            {
                // do nothing. We're waiting until both threads are done.
            }

        }

        public static int GetThreadDelay1Setting()
        {
            var reader = new AppSettingsReader();
            try
            {
                var delay1Setting = reader.GetValue("Thread1Delay", typeof(int));
                return (int)delay1Setting;
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("Configurable setting Thread1Delay is missing from the application configuration (app.config in the project, or ChallengeConsoleApp.exe.config in the binaries folder) file. Default of 1000 will be used.");
                return 1000;
            }
        }

        public static int GetThreadDelay2Setting()
        {
            var reader = new AppSettingsReader();
            try
            {
                var delay2Setting = reader.GetValue("Thread2Delay", typeof(int));
                return (int)delay2Setting;
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("Configurable setting Thread2Delay is missing from the application configuration (app.config in the project, or ChallengeConsoleApp.exe.config in the binaries folder) file. Default of 500 will be used.");
                return 500; 
            }
        }

        public static string GetLocalhostBaseAddressSetting()
        {
            var reader = new AppSettingsReader();
            try
            {
                var baseURL = reader.GetValue("localhostbaseURL", typeof(string));
                return baseURL.ToString();
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("Configurable setting localhostbaseURL is missing from the application configuration (app.config in the project, or ChallengeConsoleApp.exe.config in the binaries folder) file. Please enter a localhost string with port, for example http://localhost:58523");
                return "";
            }
        }

        public static void WriteThreadDelegate(object info)
        {
            // This receives the value passed into the Thread.Start method.
            ListOfIntsAndDelayStruct intsAndDelay = (ListOfIntsAndDelayStruct)info;

            List<int> intList = new List<int>(intsAndDelay.listOfInts);
            int sleepValue = intsAndDelay.delayMilliseconds;
            foreach (var item in intList)
            {
                Thread.Sleep(sleepValue);
                Console.WriteLine($"{Thread.CurrentThread.Name}: {item}");
            }

        }


    }



}


