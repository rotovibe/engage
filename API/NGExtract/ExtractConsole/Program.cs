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
            ETLProcessor pro = new ETLProcessor();
            pro.Rebuild();
        }
    }
}
