using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.API.DataDomain.Search.DTO
{
    public static class FileLog
    {
        private static readonly object _sync = new object();

        public static string GetTempPath()
        {
            string path = System.Environment.GetEnvironmentVariable("TEMP");
            if (!path.EndsWith("\\")) path += "\\";
            return path;
        }

        //public static void LogMessageToFile(string msg)
        //{
        //    lock (_sync)
        //    {
        //        StreamWriter sw = File.AppendText(GetTempPath() + "SearchService_Log_File.txt");
        //        try
        //        {
        //            var logLine = String.Format("{0:G}: {1}.", DateTime.Now, msg);
        //            sw.WriteLine(logLine);
        //        }
        //        finally
        //        {
        //            sw.Close();
        //        }
        //    }
        //}

        public static void LogMessageToFile(string msg)
        {
            var logLine = String.Format("{0:G}: {1}.", DateTime.Now, msg);
            //Trace.TraceInformation("Search Error");
            Trace.TraceError(logLine);
        }
    }
}
