using Phytel.Data.ETL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtractConsole
{
    public class Program
    {
        static void Main(string[] args)
        {
            ETLProcessor pro = new ETLProcessor("InHealth001");
            pro.EtlEvent += pro_EtlEvent;
            pro.Rebuild();
            Console.WriteLine("Process Finished");
            Console.ReadLine();
        }

        static void pro_EtlEvent(object sender, ETLEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
