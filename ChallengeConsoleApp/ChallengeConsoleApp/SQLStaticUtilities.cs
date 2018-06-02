using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChallengeConsoleApp
{
    public class SQLStaticUtilities
    {
        /// <summary>
        /// Insert log data into the SQL database table.
        /// </summary>

        //- StartTimeUTC -> time in UTC when the request was sent to the server
        //- EndTimeUTC -> time in UTC when a response was received from the server (or a timeout occurred)
        //- HTTPStatusCode -> response HTTP Status Code (if available)
        //- Response received from the server as a string (ASCII Encoding should be fine)
        //- Status -> ( 1 -> HTTP StatusCode 200 response received from the server, 2 -> Another HTTP StatusCode received from the server, -999 timeout)

        public static void AddLog(DateTime starttimeutc, DateTime endtimeutc, int httpstatuscode, string datastring, int status, string statusstring)
        {
            using (SqlConnection con = new SqlConnection(
                GetConnectionString()))
            {
                con.Open();
                try
                {
                    using (SqlCommand command = new SqlCommand(
                        "INSERT INTO server_response_log VALUES(@StartTimeUTC, @EndTimeUTC, @HTTPStatusCode, @DataString, @Status, @StatusString)", con))
                    {
                        command.Parameters.Add(new SqlParameter("StartTimeUTC", starttimeutc));
                        command.Parameters.Add(new SqlParameter("EndTimeUTC", endtimeutc));
                        command.Parameters.Add(new SqlParameter("HTTPStatusCode", httpstatuscode));
                        command.Parameters.Add(new SqlParameter("DataString", datastring));
                        command.Parameters.Add(new SqlParameter("Status", status));
                        command.Parameters.Add(new SqlParameter("StatusString", statusstring));
                        command.ExecuteNonQuery();
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Count not insert. Exception: " + ex.Message);
                }
            }
        }


        private static string GetConnectionString()
        {
            var reader = new AppSettingsReader();
            try
            {
                var connString = reader.GetValue("connectionstring", typeof(string));
                return (string)connString;
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine("Configurable setting 'connectionstring' (the SQL Server connection string) is missing from the application configuration (app.config in the project, or ChallengeConsoleApp.exe.config in the binaries folder) file.");
                return "";
            }
        }

    }
}

