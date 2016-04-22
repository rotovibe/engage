using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Windows.Forms;

namespace AdHocReporting
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args.Length > 0)
            {
                if (args[0].ToUpper() == "RUN")
                {
                    Reporter _reporter = new Reporter();

                    _reporter.DataDomainURL = ConfigurationManager.AppSettings.Get("URL").ToString();
                    _reporter.ContractNumber = ConfigurationManager.AppSettings.Get("Contract").ToString();
                    _reporter.UserId = ConfigurationManager.AppSettings.Get("UserId").ToString();
                    _reporter.ConfigFile = ConfigurationManager.AppSettings.Get("ConfigFile").ToString();
                    _reporter.OutputPath = ConfigurationManager.AppSettings.Get("OutputPath").ToString();
                    if (_reporter.OutputPath.EndsWith(@"\") == false)
                        _reporter.OutputPath += @"\";

                    _reporter.LoadLookUps();
                    _reporter.BuildProgramReport();
                    _reporter.BuildActionReport();

                    Application.Exit();
                }
                else
                    Application.Run(new Form1());
            }
            else
            {
                Application.Run(new Form1());
            }
        }
    }
}
