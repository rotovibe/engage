using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ExplorysExcelImporter
{
    class ExcelFileListener
    {
        static void Main(string[] args)
        {
            string loc = System.Configuration.ConfigurationManager.AppSettings.Get("filelocation");
            using (Phytel.Network.Impersonator imp = new Phytel.Network.Impersonator("explorysuser", "\\", "explorys@1"))
            {
                string[] files = System.IO.Directory.GetFiles(loc, "*.xls", SearchOption.AllDirectories);
                foreach (string file in files)
                {
                    ExplorysImport.ExplorysImport importer = new ExplorysImport.ExplorysImport(
                        System.Configuration.ConfigurationManager.AppSettings.Get("DataDomainURL"),
                        "NG",
                        1.0,
                        "Explorys001",
                        "000000000000000000000000");

                    importer.ImportFile(file);

                    System.IO.Directory.Move(file, file.Replace(".xls", ".don"));
                }
            }
        }
    }
}
