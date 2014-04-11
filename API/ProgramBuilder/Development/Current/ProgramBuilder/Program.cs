using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProgramBuilder
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
//#if DEBUG
//            Application.Run(new StepListForm());

//#else
//            Application.Run(new ProgramForm());
//#endif

            Application.Run(new ProgramForm());
        }
    }
}
