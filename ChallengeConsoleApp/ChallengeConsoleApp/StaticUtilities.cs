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
                Console.WriteLine("Configurable setting Thread1Delay is missing from the application configuration (app.config) file. Default of 1000 will be used.");
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
                Console.WriteLine("Configurable setting Thread2Delay is missing from the application configuration (app.config) file. Default of 500 will be used.");
                return 500; 
            }
        }

        public static void Write1(object info)
        {
            // This receives the value passed into the Thread.Start method.
            ListOfIntsAndDelayStruct x = (ListOfIntsAndDelayStruct)info;

            List<int> intList = new List<int>(x.listOfInts);
            int sleepValue = x.delayMilliseconds;
            foreach (var item in intList)
            {
                Thread.Sleep(sleepValue);
                Console.WriteLine(item);
            }

        }


    }



}


