using System;
using System.Collections.Generic;
using System.Diagnostics;
using Phytel.API.Interface;
using ServiceStack.ServiceClient.Web;

namespace Audit.Tasks
{
    public class EndpointTest
    {
        string _token = "";
        string _version = "";
        string _contractnumber = "";
        string _patientid = "";
        string _patientprogramid = "";
        string _cohortid = "";
        string _typename = "";
        string _flagged = "";
        string appdomainbase = @"http://localhost:888/Nightingale";
        string _patientgoalid = "";
        string _id = "";

        List<string> routes = new List<string>();
        
        
        public EndpointTest(string sessionToken, string version, string contractnumber, string patientid = "", 
                                        string cohortid = "", string patientprogramid = "", string typename = "", string flagged = "", string patientgoalid = "", string id = "")
        {
            _token = sessionToken;
            _version = version;
            _contractnumber = contractnumber;
            _patientid = patientid;
            _cohortid = cohortid;
            _patientprogramid = patientprogramid;
            _typename = typename;
            _flagged = flagged;
            _patientgoalid = patientgoalid;
            _id = id;
        }

        private List<string> GetEndpoints_GET()
        {
            routes.Clear();

            //GetPatientResponse
            routes.Add(string.Format("{2}/{0}/{1}/Patient/[PatientID]?token={3}", _version, _contractnumber, appdomainbase, _token).Replace("[PatientID]", _patientid));

            //GetAllPatientProblemsResponse
            routes.Add(string.Format("{2}/{0}/{1}/Patient/[PatientID]/Problems?token={3}", _version, _contractnumber, appdomainbase, _token).Replace("[PatientID]", _patientid));

            //GetAllProblemsResponse
            routes.Add(string.Format("{2}/{0}/{1}/Lookup/Problems?token={3}", _version, _contractnumber, appdomainbase, _token).Replace("[PatientID]", _patientid));

            //GetAllCohortsResponse
            routes.Add(string.Format("{2}/{0}/{1}/cohorts?token={3}", _version, _contractnumber, appdomainbase, _token));

            //GetCohortPatientsResponse
            routes.Add(string.Format("{2}/{0}/{1}/cohortpatients/[CohortId]?token={3}", _version, _contractnumber, appdomainbase, _token).Replace("[CohortId]", _cohortid));

            //GetAllSettingsResponse
            routes.Add(string.Format("{2}/{0}/{1}/settings?token={3}", _version, _contractnumber, appdomainbase, _token));

            //GetActiveProgramsResponse
            routes.Add(string.Format("{2}/{0}/{1}/Programs/Active?token={3}", _version, _contractnumber, appdomainbase, _token));

            //GetPatientProgramDetailsSummaryResponse
            routes.Add(string.Format("{2}/{0}/{1}/Patient/[PatientId]/Program/[PatientProgramId]?token={3}", _version, _contractnumber, appdomainbase, _token).Replace("[PatientId]", _patientid).Replace("[PatientProgramId]", _patientprogramid));

            //GetPatientProgramsResponse
            routes.Add(string.Format("{2}/{0}/{1}/Patient/[PatientId]/Programs?token={3}", _version, _contractnumber, appdomainbase, _token).Replace("[PatientId]", _patientid));

            //GetContactResponse
            routes.Add(string.Format("{2}/{0}/{1}/Patient/[PatientId]/Contact?token={3}", _version, _contractnumber, appdomainbase, _token).Replace("[PatientId]", _patientid));

            //GetAllCommModesResponse
            routes.Add(string.Format("{2}/{0}/{1}/lookup/commmodes?token={3}", _version, _contractnumber, appdomainbase, _token));

            //GetAllStatesResponse
            routes.Add(string.Format("{2}/{0}/{1}/lookup/states?token={3}", _version, _contractnumber, appdomainbase, _token));

            //GetAllTimesOfDaysResponse
            routes.Add(string.Format("{2}/{0}/{1}/lookup/timesofdays?token={3}", _version, _contractnumber, appdomainbase, _token));

            //GetAllTimeZonesResponse
            routes.Add(string.Format("{2}/{0}/{1}/lookup/timezones?token={3}", _version, _contractnumber, appdomainbase, _token));

            //GetAllCommTypesResponse
            routes.Add(string.Format("{2}/{0}/{1}/lookup/commtypes?token={3}", _version, _contractnumber, appdomainbase, _token));

            //GetAllLanguagesResponse
            routes.Add(string.Format("{2}/{0}/{1}/lookup/languages?token={3}", _version, _contractnumber, appdomainbase, _token));

            //GetLookUpsResponse
            routes.Add(string.Format("{2}/{0}/{1}/lookup/[TypeName]?token={3}", _version, _contractnumber, appdomainbase, _token).Replace("[TypeName]", _typename));

            ////GetInitializeGoalResponse
            //routes.Add(string.Format("{2}/{0}/{1}/Patient/[PatientId]/Goal/Initialize", _version, _contractnumber, appdomainbase, _token).Replace("[PatientId]", _patientid));

            ////GetInitializeBarrierResponse
            //routes.Add(string.Format("{2}/{0}/{1}/Patient/[PatientId]/Goal/[PatientGoalId]/Barrier/Initialize", _version, _contractnumber, appdomainbase, _token).Replace("[PatientId]", _patientid).Replace("[PatientGoalId]", _patientgoalid));
            
            ////GetInitializeTaskResponse
            //routes.Add(string.Format("{2}/{0}/{1}/Patient/[PatientId]/Goal/[PatientGoalId]/Task/Initialize", _version, _contractnumber, appdomainbase, _token).Replace("[PatientId]", _patientid).Replace("[PatientGoalId]", _patientgoalid));

            ////GetInitializeInterventionResponse
            //routes.Add(string.Format("{2}/{0}/{1}/Patient/[PatientId]/Goal/[PatientGoalId]/Intervention/Initialize", _version, _contractnumber, appdomainbase, _token).Replace("[PatientId]", _patientid).Replace("[PatientGoalId]", _patientgoalid));

            ////GetPatientGoalResponse
            //routes.Add(string.Format("{2}/{0}/{1}/Patient/[PatientId]/Goal/[Id]", _version, _contractnumber, appdomainbase, _token).Replace("[PatientId]", _patientid).Replace("[Id]", _id));

            ////GetAllPatientGoalsResponse
            //routes.Add(string.Format("{2}/{0}/{1}/Patient/[PatientId]/Goals", _version, _contractnumber, appdomainbase, _token).Replace("[PatientId]", _patientid));


             return routes;
        }

        private List<string> GetEndpoints_POST()
        {
            routes.Clear();

            //GetPatientResponse
            routes.Add(string.Format("{2}/{0}/{1}/Patient?token={3}", _version, _contractnumber, appdomainbase, _token));

            //PatientFlaggedUpdateResponse
            routes.Add(string.Format("{2}/{0}/{1}/patient/[PatientId]/flagged/[Flagged]?token={3}", _version, _contractnumber, appdomainbase, _token).Replace("[PatientId]",_patientid).Replace("[Flagged]", _flagged));

            //PatientDetailsUpdateResponse
            routes.Add(string.Format("{2}/{0}/{1}/patient/Update?token={3}", _version, _contractnumber, appdomainbase, _token));

            //PatientToProgramsResponse
            routes.Add(string.Format("{2}/{0}/{1}/Patient/[PatientId]/Programs?token={3}", _version, _contractnumber, appdomainbase, _token).Replace("[PatientId]", _patientid));

            //UpdateContactResponse
            routes.Add(string.Format("{2}/{0}/{1}/Contact?token={3}", _version, _contractnumber, appdomainbase, _token));
            routes.Add(string.Format("{2}/{0}/{1}/Patient/Contact?token={3}", _version, _contractnumber, appdomainbase, _token));


            return routes;
        }

        
        public void HitEndpoints_GET(bool isGet)
        {
            List<string> endpoints = isGet ? GetEndpoints_GET() : GetEndpoints_POST();
            int i = 0;

            //IRestClient client = new JsonServiceClient();

            //JsonServiceClient client = new JsonServiceClient();
            //client.HttpMethod = HttpMethods.Get;

            //JsonServiceClient.h
            //client.Headers.Add("x-Phytel-UserID", "bb241c64-a0ff-4e01-ba5f-4246ef50780e");

            //JsonServiceClient.HttpWebRequestFilter filter

            //JsonServiceClient client.HttpHeaders.HttpWebRequestFilter = x =>
            //        x.Headers.Add(string.Format("{0}: {1}", "x-Phytel-UserID", "Valid User ID"));

            IDomainResponse sysResponse;

            foreach(string endpoint in endpoints)
            {
                try
                {
                    i += 1;
                    //Console.WriteLine("Calling {0}", endpoint);
                    //sysResponse = client.Get<IDomainResponse>(endpoint);
                    
                }
                catch (WebServiceException wex)
                {
                    if (wex.StatusCode.ToString() == "404")
                        Debug.WriteLine(String.Format("{0} not found...", endpoint));
                }
                catch (Exception ex)
                {


                }

                //Console.WriteLine("Called {0} routes...", i);
            }

            Console.WriteLine("Called {0} routes...", i);
        }

      
        public void HitEndpoints_POST(List<string> endpoints)
        { 
        
        }
    }
}
