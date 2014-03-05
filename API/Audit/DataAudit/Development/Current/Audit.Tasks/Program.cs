using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Phytel.API.Audit.DataAudit;
//using Audit.DataAudit.Service;

namespace Audit.Tasks
{
    class Program
    {
        static string _token = "53079028d2d8e91748f416cc";
        static string _version = "v1";
        static string _contractnumber = "inhealth001";
        static string _patientid = "52f55859072ef709f84e5e20";
        static string _cohortid = "528ed977072ef70e10099685";
        static string _patientprogramid = "52f56d9fd6a4850fd025fb67";
        static string _typename = "CommMode";
        static string _flagged = "true";
        static string _patientgoalid = "0";
        static string _id = "0";

        static void Main(string[] args)
        {
            EndpointTest test = new EndpointTest(_token, _version, _contractnumber, _patientid, _cohortid,
                                                                        _patientprogramid, _typename, _flagged, _patientgoalid, _id);

            test.HitEndpoints_GET(true);

            Console.ReadLine();
        }
    }
}
