using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.PatientProblem;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.PatientProblem.DTO;
using System.IO;
using System.Reflection;
namespace Phytel.API.DataDomain.PatientProblem.Tests
{
    [TestClass()]
    public class DataPatientProblemManager_Tests
    {
        [TestMethod()]
        public void CreateProblemsScript()
        {
            string context = "NG";
            string contractNumber = "InHealth001";
            string userId = "000000000000000000000000";
            Dictionary<string, string> observationDict = new Dictionary<string, string>();

            InitializeObservationDictionary(observationDict);
            GetAllPatientProblemsDataRequest request = new GetAllPatientProblemsDataRequest { Context = context, ContractNumber = contractNumber, UserId = userId };

            // get all patient problems
            IPatientProblemRepository<PatientProblemData> repo = 
                PatientProblemRepositoryFactory<PatientProblemData>.GetPatientProblemRepository(request.ContractNumber, request.Context, request.UserId);

            List<MEPatientProblem> pp = (List<MEPatientProblem>)repo.SelectAll();

            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            string mydocpath = Path.GetDirectoryName(path);

            StringBuilder sb = new StringBuilder();

            foreach (MEPatientProblem mpp in pp)
            {

                sb.AppendLine("{");
                sb.AppendLine(" \"oid\" : ObjectId(\"" + GetProblemObservationId(observationDict, mpp.ProblemID.ToString()) + "\"),");
                sb.AppendLine(" \"pid\" : ObjectId(\"" + mpp.PatientID.ToString() + "\"),");
                sb.AppendLine(" \"st\" : 0,");
                sb.AppendLine(" \"v\" : 1.0,");
                sb.AppendLine(" \"uby\" : null,");
                sb.AppendLine(" \"del\" : false,");
                sb.AppendLine(" \"ttl\" : null,");
                sb.AppendLine(" \"uon\" : ISODate(\"" + Convert.ToDateTime(mpp.LastUpdatedOn).ToString("o") + "\"),");
                sb.AppendLine(" \"rcby\" : ObjectId(\"" + mpp.RecordCreatedBy.ToString() + "\"),");
                sb.AppendLine(" \"rcon\" : ISODate(\"" + mpp.RecordCreatedOn.ToString("o") + "\"),");
                sb.AppendLine("}");
            }

            using (StreamWriter outfile = new StreamWriter(mydocpath + @"\PatientProblemImport_" + System.DateTime.Now.ToString(@"yyyy_MM_dd") + ".txt"))
            {
                outfile.Write(sb.ToString());
            }

        }

        private void InitializeObservationDictionary(Dictionary<string, string> observationDict)
        {
            observationDict.Add("528a66d6d4332317acc5095d", "534838c3fe7a5910fccab75f");
            observationDict.Add("528a66e3d4332317acc5095e", "533ed16cd4332307bc592baa");
            observationDict.Add("528a6703d4332317acc50961", "533ed16cd4332307bc592ba9");
            observationDict.Add("528a6709d4332317acc50962", "533ed16ed4332307bc592bb7");
            observationDict.Add("528a671ad4332317acc50965", "534838d1fe7a5910fccab760");
            observationDict.Add("528a6723d4332317acc50966", "534838defe7a5910fccab761");
            observationDict.Add("528a6729d4332317acc50967", "533ed16cd4332307bc592ba7");
            observationDict.Add("532c4d25d6a4851308125aa2", "533ed16dd4332307bc592bac");
            observationDict.Add("528a672fd4332317acc50968", "533ed16dd4332307bc592bad");
            observationDict.Add("528a6735d4332317acc50969", "533ed16cd4332307bc592ba6");
            observationDict.Add("528a673cd4332317acc5096a", "533ed16cd4332307bc592bab");
            observationDict.Add("528a6715d4332317acc50964", "533ed16dd4332307bc592bae");
            observationDict.Add("528a66fdd4332317acc50960", "533ed16cd4332307bc592ba8");
            observationDict.Add("528a66f4d4332317acc5095f", "533ed16dd4332307bc592baf");
            observationDict.Add("528a670ed4332317acc50963", "533ed16ed4332307bc592bba");
            observationDict.Add("528a66ced4332317acc5095c", "533ed16cd4332307bc592ba5");
        }

        private static string GetProblemObservationId(Dictionary<string, string> dict, string probId)
        {
            string observationId = string.Empty;
            observationId = dict[probId];
            return observationId;
        }
    }
}
