using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using System.Windows.Forms;
using NightingaleImport.Configuration;

namespace NightingaleImport
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var contractNumber = Configurations.contractNumber;
            var enhancedFeauresContracts = Configurations.enhancedFeauresContracts;
            if (enhancedFeauresContracts.Contains(contractNumber))
            {
                Application.Run(new FormPatientsImport());
            }
            else
            {
                Application.Run(new Form1());
            }
       
        }
    }
}
