using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.SessionState;
using System.Windows.Forms;
using Phytel.API.Common.CustomObject;
using Phytel.API.DataDomain.CareMember.DTO;
using Phytel.API.DataDomain.Contact.DTO;
using Phytel.API.DataDomain.LookUp.DTO;
using Phytel.API.DataDomain.Patient.DTO;
using Phytel.API.DataDomain.PatientSystem.DTO;
using Microsoft.VisualBasic.FileIO;
using Phytel.API.DataDomain.Contact.DTO.CareTeam;

namespace NGDataImport
{
    public class Importer
    {
        
        private List<CommTypeData> typesLookUp = new List<CommTypeData>();

        public string Url { get; set; }
        public string Context { get; set; }
        public double Version { get; set; }
        public string ContractNumber { get; set; }
        public string HeaderUserId { get; set; }

        public Importer(string url, string context, double version, string contract, string headerUserId)
        {
            Url = url;
            Context = context;
            Version = version;
            ContractNumber = contract;
            HeaderUserId = headerUserId;
        }

        public TimeZoneData GetDefaultTimeZone()
        {
            Uri zoneUri = new Uri(string.Format("{0}/LookUp/{1}/{2}/{3}/TimeZone/Default?UserId={4}",
                                                    Url,
                                                    Context,
                                                    Version,
                                                    ContractNumber,
                                                    HeaderUserId));
            HttpClient zoneClient = GetHttpClient(zoneUri);

            GetTimeZoneDataRequest zoneRequest = new GetTimeZoneDataRequest
            {
                Version = Version,
                Context = Context,
                ContractNumber = ContractNumber
            };

            DataContractJsonSerializer zoneJsonSer = new DataContractJsonSerializer(typeof(GetTimeZoneDataRequest));
            MemoryStream zoneMs = new MemoryStream();
            zoneJsonSer.WriteObject(zoneMs, zoneRequest);
            zoneMs.Position = 0;

            //use a Stream reader to construct the StringContent (Json) 
            StreamReader zoneSr = new StreamReader(zoneMs);
            StringContent zoneContent = new StringContent(zoneSr.ReadToEnd(), System.Text.Encoding.UTF8, "application/json");

            //Post the data 
            var zoneResponse = zoneClient.GetStringAsync(zoneUri);
            var zoneResponseContent = zoneResponse.Result;

            string zoneResponseString = zoneResponseContent;
            GetTimeZoneDataResponse responseZone = null;

            using (var zoneMsResponse = new MemoryStream(Encoding.Unicode.GetBytes(zoneResponseString)))
            {
                var zoneSerializer = new DataContractJsonSerializer(typeof(GetTimeZoneDataResponse));
                responseZone = (GetTimeZoneDataResponse)zoneSerializer.ReadObject(zoneMsResponse);
            }
            return responseZone.TimeZone;
        }

        public List<IdNamePair> GetModes()
        {
            Uri modesUri = new Uri(string.Format("{0}/LookUp/{1}/{2}/{3}/commmodes?UserId={4}",
                                                    Url,
                                                    Context,
                                                    Version,
                                                    ContractNumber,
                                                    HeaderUserId));
            HttpClient modesClient = GetHttpClient(modesUri);

            GetAllCommModesDataRequest modesRequest = new GetAllCommModesDataRequest
            {
                Version = Version,
                Context = Context,
                ContractNumber = ContractNumber
            };

            DataContractJsonSerializer modesJsonSer = new DataContractJsonSerializer(typeof(GetAllCommModesDataRequest));
            MemoryStream modesMs = new MemoryStream();
            modesJsonSer.WriteObject(modesMs, modesRequest);
            modesMs.Position = 0;

            //use a Stream reader to construct the StringContent (Json) 
            StreamReader modesSr = new StreamReader(modesMs);
            StringContent modesContent = new StringContent(modesSr.ReadToEnd(), System.Text.Encoding.UTF8, "application/json");

            //Post the data 
            var modesResponse = modesClient.GetStringAsync(modesUri);
            var modesResponseContent = modesResponse.Result;

            string modesResponseString = modesResponseContent;
            GetAllCommModesDataResponse responseModes = null;

            using (var modesMsResponse = new MemoryStream(Encoding.Unicode.GetBytes(modesResponseString)))
            {
                var modesSerializer = new DataContractJsonSerializer(typeof(GetAllCommModesDataResponse));
                responseModes = (GetAllCommModesDataResponse)modesSerializer.ReadObject(modesMsResponse);
            }
            return responseModes.CommModes;
        }
        
        public string GetType(string type)
        {
            Uri typesUri = new Uri(string.Format("{0}/LookUp/{1}/{2}/{3}/commtypes?UserId={4}",
                                                    Url,
                                                    Context,
                                                    Version,
                                                    ContractNumber,
                                                    HeaderUserId));
            HttpClient typesClient = GetHttpClient(typesUri);

            GetAllCommTypesDataRequest typesRequest = new GetAllCommTypesDataRequest
            {
                Version = Version,
                Context = Context,
                ContractNumber = ContractNumber
            };

            DataContractJsonSerializer typesJsonSer = new DataContractJsonSerializer(typeof(GetAllCommTypesDataRequest));
            MemoryStream typesMs = new MemoryStream();
            typesJsonSer.WriteObject(typesMs, typesRequest);
            typesMs.Position = 0;

            //use a Stream reader to construct the StringContent (Json) 
            StreamReader typesSr = new StreamReader(typesMs);
            StringContent typesContent = new StringContent(typesSr.ReadToEnd(), System.Text.Encoding.UTF8, "application/json");

            //Post the data 
            var typesResponse = typesClient.GetStringAsync(typesUri);
            var typesResponseContent = typesResponse.Result;

            string typesResponseString = typesResponseContent;
            GetAllCommTypesDataResponse responseTypes = null;

            using (var typesMsResponse = new MemoryStream(Encoding.Unicode.GetBytes(typesResponseString)))
            {
                var typesSerializer = new DataContractJsonSerializer(typeof(GetAllCommTypesDataResponse));
                responseTypes = (GetAllCommTypesDataResponse)typesSerializer.ReadObject(typesMsResponse);
            }
            typesLookUp = responseTypes.CommTypes;
            string t = "";
            foreach (CommTypeData c in typesLookUp)
            {
                if (String.Compare(type, c.Name, true) == 0)
                {
                    t = c.Id;
                }
            }
            return t;
        }

        public string GetState(string state)
        {
            Uri statesUri = new Uri(string.Format("{0}/LookUp/{1}/{2}/{3}/states?UserId={4}",
                                                    Url,
                                                    Context,
                                                    Version,
                                                    ContractNumber,
                                                    HeaderUserId));
            HttpClient statesClient = GetHttpClient(statesUri);

            GetAllStatesDataRequest statesRequest = new GetAllStatesDataRequest
            {
                Version = Version,
                Context = Context,
                ContractNumber = ContractNumber
            };

            DataContractJsonSerializer statesJsonSer = new DataContractJsonSerializer(typeof(GetAllStatesDataRequest));
            MemoryStream statesMs = new MemoryStream();
            statesJsonSer.WriteObject(statesMs, statesRequest);
            statesMs.Position = 0;

            //use a Stream reader to construct the StringContent (Json) 
            StreamReader statesSr = new StreamReader(statesMs);
            StringContent statesContent = new StringContent(statesSr.ReadToEnd(), System.Text.Encoding.UTF8, "application/json");

            //Post the data 
            var statesResponse = statesClient.GetStringAsync(statesUri);
            var statesResponseContent = statesResponse.Result;

            string statesResponseString = statesResponseContent;
            GetAllStatesDataResponse responseStates = null;

            using (var statesMsResponse = new MemoryStream(Encoding.Unicode.GetBytes(statesResponseString)))
            {
                var statesSerializer = new DataContractJsonSerializer(typeof(GetAllStatesDataResponse));
                responseStates = (GetAllStatesDataResponse)statesSerializer.ReadObject(statesMsResponse);
            }
            
            List<StateData> statesLookUp = responseStates.States;
            string s = "";
            foreach (StateData st in statesLookUp)
            {
                if ((st.Name == state)
                    || (st.Code == state))
                {
                    s = st.Id;
                }
            }
            return s;
        }

        public string GetFirstTypeLookUp()
        {
            Uri typesUri = new Uri(string.Format("{0}/LookUp/{1}/{2}/{3}/commtypes?UserId={4}",
                                                    Url,
                                                    Context,
                                                    Version,
                                                    ContractNumber,
                                                    HeaderUserId));
            HttpClient typesClient = GetHttpClient(typesUri);

            GetAllCommTypesDataRequest typesRequest = new GetAllCommTypesDataRequest
            {
                Version = Version,
                Context = Context,
                ContractNumber = ContractNumber
            };

            DataContractJsonSerializer typesJsonSer = new DataContractJsonSerializer(typeof(GetAllCommTypesDataRequest));
            MemoryStream typesMs = new MemoryStream();
            typesJsonSer.WriteObject(typesMs, typesRequest);
            typesMs.Position = 0;

            //use a Stream reader to construct the StringContent (Json) 
            StreamReader typesSr = new StreamReader(typesMs);
            StringContent typesContent = new StringContent(typesSr.ReadToEnd(), System.Text.Encoding.UTF8, "application/json");

            //Post the data 
            var typesResponse = typesClient.GetStringAsync(typesUri);
            var typesResponseContent = typesResponse.Result;

            string typesResponseString = typesResponseContent;
            GetAllCommTypesDataResponse responseTypes = null;

            using (var typesMsResponse = new MemoryStream(Encoding.Unicode.GetBytes(typesResponseString)))
            {
                var typesSerializer = new DataContractJsonSerializer(typeof(GetAllCommTypesDataResponse));
                responseTypes = (GetAllCommTypesDataResponse)typesSerializer.ReadObject(typesMsResponse);
            }
            typesLookUp = responseTypes.CommTypes;
            return typesLookUp[0].Id;
        }

        public PutPatientDataResponse InsertPatient(PutPatientDataRequest putPatientRequest)
        {
            try
            {
                //Patient
                Uri theUri = new Uri(string.Format("{0}/Patient/{1}/{2}/{3}/Patient/Insert?UserId={4}",
                                                     Url,
                                                     Context,
                                                     Version,
                                                     ContractNumber,
                                                     HeaderUserId));

                HttpClient client = GetHttpClient(theUri);

                DataContractJsonSerializer jsonSer = new DataContractJsonSerializer(typeof(PutPatientDataRequest));

                // use the serializer to write the object to a MemoryStream 
                MemoryStream ms = new MemoryStream();
                jsonSer.WriteObject(ms, putPatientRequest);
                ms.Position = 0;


                //use a Stream reader to construct the StringContent (Json) 
                StreamReader sr = new StreamReader(ms);

                StringContent theContent = new StringContent(sr.ReadToEnd(), System.Text.Encoding.UTF8, "application/json");

                //Post the data 
                var response = client.PutAsync(theUri, theContent);
                var responseContent = response.Result.Content;

                string responseString = responseContent.ReadAsStringAsync().Result;
                PutPatientDataResponse responsePatient = null;

                using (var msResponse = new MemoryStream(Encoding.Unicode.GetBytes(responseString)))
                {
                    var serializer = new DataContractJsonSerializer(typeof(PutPatientDataResponse));
                    responsePatient = (PutPatientDataResponse)serializer.ReadObject(msResponse);
                }

                return responsePatient;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public InsertPatientSystemDataResponse InsertPatientSystem(InsertPatientSystemDataRequest request)
        {
            //[Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/PatientSystem", "POST")]
            Uri theUriPS = new Uri(string.Format("{0}/PatientSystem/{1}/{2}/{3}/Patient/{4}/PatientSystem?UserId={5}",
                                                   Url,
                                                   Context,
                                                   Version,
                                                   ContractNumber,
                                                   request.PatientId,
                                                   HeaderUserId));
            HttpClient clientPS = GetHttpClient(theUriPS);

            DataContractJsonSerializer jsonSerPS = new DataContractJsonSerializer(typeof(InsertPatientSystemDataRequest));

            // use the serializer to write the object to a MemoryStream 
            MemoryStream msPS = new MemoryStream();
            jsonSerPS.WriteObject(msPS, request);
            msPS.Position = 0;


            //use a Stream reader to construct the StringContent (Json) 
            StreamReader srPS = new StreamReader(msPS);

            StringContent theContentPS = new StringContent(srPS.ReadToEnd(), System.Text.Encoding.UTF8, "application/json");

            //Post the data 
            var responsePS = clientPS.PostAsync(theUriPS, theContentPS);
            var responseContentPS = responsePS.Result.Content;

            string responseStringPS = responseContentPS.ReadAsStringAsync().Result;
            InsertPatientSystemDataResponse responsePatientPS = null;

            using (var msResponsePS = new MemoryStream(Encoding.Unicode.GetBytes(responseStringPS)))
            {
                var serializerPS = new DataContractJsonSerializer(typeof(InsertPatientSystemDataResponse));
                responsePatientPS = (InsertPatientSystemDataResponse)serializerPS.ReadObject(msResponsePS);
            }

            return responsePatientPS;
        }

        public GetPatientSystemDataResponse GetPatientSystem(GetPatientSystemDataRequest request)
        {
            //[Route("/{Context}/{Version}/{ContractNumber}/PatientSystem/{Id}", "GET")]
            Uri theUriPS = new Uri(string.Format("{0}/PatientSystem/{1}/{2}/{3}/PatientSystem/{4}?UserId={5}",
                                                   Url,
                                                   Context,
                                                   Version,
                                                   ContractNumber,
                                                   request.Id,
                                                   HeaderUserId));
            HttpClient client = GetHttpClient(theUriPS);

            DataContractJsonSerializer modesJsonSer = new DataContractJsonSerializer(typeof(GetPatientSystemDataRequest));
            MemoryStream ms = new MemoryStream();
            modesJsonSer.WriteObject(ms, request);
            ms.Position = 0;

            //use a Stream reader to construct the StringContent (Json) 
            StreamReader modesSr = new StreamReader(ms);
            StringContent modesContent = new StringContent(modesSr.ReadToEnd(), System.Text.Encoding.UTF8, "application/json");

            //Post the data 
            var response = client.GetStringAsync(theUriPS);
            var responseContent = response.Result;

            string modesResponseString = responseContent;
            GetPatientSystemDataResponse getPatientSystemDataResponse = null;

            using (var memStream = new MemoryStream(Encoding.Unicode.GetBytes(modesResponseString)))
            {
                var modesSerializer = new DataContractJsonSerializer(typeof(GetPatientSystemDataResponse));
                getPatientSystemDataResponse = (GetPatientSystemDataResponse)modesSerializer.ReadObject(memStream);
            }
            return getPatientSystemDataResponse;
        }

        public UpdatePatientSystemDataResponse UpdatePatientSystem(UpdatePatientSystemDataRequest request)
        {
            //[Route("/{Context}/{Version}/{ContractNumber}/Patient/{PatientId}/PatientSystem/{Id}", "PUT")]
            Uri updateUri = new Uri(string.Format("{0}/PatientSystem/{1}/{2}/{3}/Patient/{4}/PatientSystem/{5}?UserId={6}",
                                                        Url,
                                                        Context,
                                                        Version,
                                                        ContractNumber,
                                                        request.PatientId,
                                                        request.Id,
                                                        HeaderUserId));
            HttpClient updateClient = GetHttpClient(updateUri);

            UpdatePatientSystemDataResponse response = null;

            DataContractJsonSerializer updateJsonSer = new DataContractJsonSerializer(typeof(UpdatePatientSystemDataRequest));

            // use the serializer to write the object to a MemoryStream 
            MemoryStream updateMs = new MemoryStream();
            updateJsonSer.WriteObject(updateMs, request);
            updateMs.Position = 0;


            //use a Stream reader to construct the StringContent (Json) 
            StreamReader updateSr = new StreamReader(updateMs);

            StringContent updateContent = new StringContent(updateSr.ReadToEnd(), System.Text.Encoding.UTF8, "application/json");

            //Post the data 
            var updateResponse = updateClient.PutAsync(updateUri, updateContent);
            var updateResponseContent = updateResponse.Result.Content;

            string updateResponseString = updateResponseContent.ReadAsStringAsync().Result;


            using (var updateMsResponse = new MemoryStream(Encoding.Unicode.GetBytes(updateResponseString)))
            {
                var updateSerializer = new DataContractJsonSerializer(typeof(UpdatePatientSystemDataResponse));
                response = (UpdatePatientSystemDataResponse)updateSerializer.ReadObject(updateMsResponse);
            }

            return response;
        }

        public PutUpdatePatientDataResponse UpsertPatient(PutUpdatePatientDataRequest putUpdatePatient, string patientId)
        {
            Uri updateUri = new Uri(string.Format("{0}/Patient/{1}/{2}/{3}/patient?UserId={4}",
                                                        Url,
                                                        Context,
                                                        Version,
                                                        ContractNumber,
                                                        HeaderUserId));
            HttpClient updateClient = GetHttpClient(updateUri);

            PutUpdatePatientDataResponse updateResponsePatient = null;

            DataContractJsonSerializer updateJsonSer = new DataContractJsonSerializer(typeof(PutUpdatePatientDataRequest));

            // use the serializer to write the object to a MemoryStream 
            MemoryStream updateMs = new MemoryStream();
            updateJsonSer.WriteObject(updateMs, putUpdatePatient);
            updateMs.Position = 0;


            //use a Stream reader to construct the StringContent (Json) 
            StreamReader updateSr = new StreamReader(updateMs);

            StringContent updateContent = new StringContent(updateSr.ReadToEnd(), System.Text.Encoding.UTF8, "application/json");

            //Post the data 
            var updateResponse = updateClient.PutAsync(updateUri, updateContent);
            var updateResponseContent = updateResponse.Result.Content;

            string updateResponseString = updateResponseContent.ReadAsStringAsync().Result;


            using (var updateMsResponse = new MemoryStream(Encoding.Unicode.GetBytes(updateResponseString)))
            {
                var updateSerializer = new DataContractJsonSerializer(typeof(PutUpdatePatientDataResponse));
                updateResponsePatient = (PutUpdatePatientDataResponse)updateSerializer.ReadObject(updateMsResponse);
            }

            return updateResponsePatient;
        }

        public InsertContactDataResponse InsertContactForAPatient(InsertContactDataRequest putContactRequest, string patientId)
        {
            //[Route("/{Context}/{Version}/{ContractNumber}/Contacts", "POST")]
            Uri contactUri = new Uri(string.Format("{0}/Contact/{1}/{2}/{3}/Contacts?UserId={4}",
                                            Url,
                                            Context,
                                            Version,
                                            ContractNumber,
                                            HeaderUserId));
            HttpClient contactClient = GetHttpClient(contactUri);

            DataContractJsonSerializer contactJsonSer = new DataContractJsonSerializer(typeof(InsertContactDataRequest));
            MemoryStream contactMs = new MemoryStream();
            contactJsonSer.WriteObject(contactMs, putContactRequest);
            contactMs.Position = 0;

            //use a Stream reader to construct the StringContent (Json) 
            StreamReader contactSr = new StreamReader(contactMs);
            StringContent contactContent = new StringContent(contactSr.ReadToEnd(), System.Text.Encoding.UTF8, "application/json");

            //Post the data 
            var contactResponse = contactClient.PostAsync(contactUri, contactContent);
            var contactResponseContent = contactResponse.Result.Content;

            string contactResponseString = contactResponseContent.ReadAsStringAsync().Result;
            InsertContactDataResponse responseContact = null;

            using (var contactMsResponse = new MemoryStream(Encoding.Unicode.GetBytes(contactResponseString)))
            {
                var contactSerializer = new DataContractJsonSerializer(typeof(InsertContactDataResponse));
                responseContact = (InsertContactDataResponse)contactSerializer.ReadObject(contactMsResponse);
            }

            return responseContact;
        }

        public SaveCareTeamDataResponse InsertCareTeam(SaveCareTeamDataRequest request)
        {
            //[Route("/{Context}/{Version}/{ContractNumber}/Contacts/{ContactId}/CareTeams", "POST")]
            Uri contactUri = new Uri(string.Format("{0}/Contact/{1}/{2}/{3}/Contacts/{4}/CareTeams?UserId={5}",
                                                 Url,
                                                 Context,
                                                 Version,
                                                 ContractNumber,
                                                 request.ContactId,
                                                 HeaderUserId));
            HttpClient client = GetHttpClient(contactUri);

            DataContractJsonSerializer jsonSer = new DataContractJsonSerializer(typeof(SaveCareTeamDataRequest));

            // use the serializer to write the object to a MemoryStream 
            MemoryStream ms = new MemoryStream();
            jsonSer.WriteObject(ms, request);
            ms.Position = 0;

            //use a Stream reader to construct the StringContent (Json) 
            StreamReader sr = new StreamReader(ms);

            StringContent theContent = new StringContent(sr.ReadToEnd(), System.Text.Encoding.UTF8, "application/json");

            //Post the data 
            var response = client.PostAsync(contactUri, theContent);
            var responseContent = response.Result.Content;

            string responseString = responseContent.ReadAsStringAsync().Result;
            SaveCareTeamDataResponse saveCareTeamDataResponse = null;

            using (var msResponse = new MemoryStream(Encoding.Unicode.GetBytes(responseString)))
            {
                var serializer = new DataContractJsonSerializer(typeof(SaveCareTeamDataResponse));
                saveCareTeamDataResponse = (SaveCareTeamDataResponse)serializer.ReadObject(msResponse);
            }

            return saveCareTeamDataResponse;
        }

        public GetContactByUserIdDataResponse GetContactByUserId(string userId)
        {
            Uri getContactUri = new Uri(string.Format("{0}/Contact/{1}/{2}/{3}/Contact/User/{4}?UserId={5}",
                                                    Url,
                                                    Context,
                                                    Version,
                                                    ContractNumber,
                                                    userId,
                                                    HeaderUserId));
            HttpClient getContactClient = GetHttpClient(getContactUri);

            GetContactByUserIdDataRequest getContactRequest = new GetContactByUserIdDataRequest
            {
                SQLUserId = userId,
                Context = Context,
                Version = Version,
                ContractNumber = ContractNumber
            };

            DataContractJsonSerializer getContactJsonSer = new DataContractJsonSerializer(typeof(GetContactByUserIdDataRequest));
            MemoryStream getContactMs = new MemoryStream();
            getContactJsonSer.WriteObject(getContactMs, getContactRequest);
            getContactMs.Position = 0;

            //use a Stream reader to construct the StringContent (Json) 
            StreamReader getContactSr = new StreamReader(getContactMs);
            StringContent getContactContent = new StringContent(getContactSr.ReadToEnd(), System.Text.Encoding.UTF8, "application/json");

            //Post the data 
            var getContactResponse = getContactClient.GetStringAsync(getContactUri);
            var getContactResponseContent = getContactResponse.Result;

            string getContactResponseString = getContactResponseContent;
            GetContactByUserIdDataResponse responseContact = null;

            using (var getContactMsResponse = new MemoryStream(Encoding.Unicode.GetBytes(getContactResponseString)))
            {
                var getContactSerializer = new DataContractJsonSerializer(typeof(GetContactByUserIdDataResponse));
                responseContact = (GetContactByUserIdDataResponse)getContactSerializer.ReadObject(getContactMsResponse);
            }
            return responseContact;
        }

        public void UpdateCohortPatientView(string patientId, string careMemberContactId)
        {
            GetCohortPatientViewResponse getResponse = GetCohortPatientView(patientId);

            if (getResponse != null && getResponse.CohortPatientView != null)
            {
                CohortPatientViewData cpvd = getResponse.CohortPatientView;
                // check to see if primary care manager's contactId exists in the searchfield
                if (!cpvd.SearchFields.Exists(sf => sf.FieldName == "PCM"))
                {
                    cpvd.SearchFields.Add(new SearchFieldData
                    {
                        Value = careMemberContactId,
                        Active = true,
                        FieldName = "PCM"
                    });
                }
                else
                {
                    cpvd.SearchFields.ForEach(sf =>
                    {
                        if (sf.FieldName == "PCM")
                        {
                            sf.Value = careMemberContactId;
                            sf.Active = true;
                        }
                    });
                }

                PutUpdateCohortPatientViewRequest request = new PutUpdateCohortPatientViewRequest
                {
                    CohortPatientView = cpvd,
                    ContractNumber = ContractNumber,
                    PatientID = patientId
                };

                PutUpdateCohortPatientViewResponse response = UpdateCohortPatientView(request, patientId);
                if (string.IsNullOrEmpty(response.CohortPatientViewId))
                    throw new Exception("Unable to update Cohort Patient View");
            }
        }

        public GetCohortPatientViewResponse GetCohortPatientView(string patientId)
        {
            Uri getCohortUri = new Uri(string.Format("{0}/Patient/{1}/{2}/{3}/patient/{4}/cohortpatientview?UserId={5}",
                                                        Url,
                                                        Context,
                                                        Version,
                                                        ContractNumber,
                                                        patientId,
                                                        HeaderUserId));

            HttpClient getCohortClient = GetHttpClient(getCohortUri);

            var getCohortResponse = getCohortClient.GetStringAsync(getCohortUri);
            var getCohortResponseContent = getCohortResponse.Result;

            string getCohortResponseString = getCohortResponseContent;
            GetCohortPatientViewResponse responseContact = null;

            using (var getCohortMsResponse = new MemoryStream(Encoding.Unicode.GetBytes(getCohortResponseString)))
            {
                var getContactSerializer = new DataContractJsonSerializer(typeof(GetCohortPatientViewResponse));
                responseContact = (GetCohortPatientViewResponse)getContactSerializer.ReadObject(getCohortMsResponse);
            }

            return responseContact;
        }

        public PutUpdateCohortPatientViewResponse UpdateCohortPatientView(PutUpdateCohortPatientViewRequest request, string patientId)
        {
            Uri cohortPatientUri = new Uri(string.Format("{0}/Patient/{1}/{2}/{3}/patient/{4}/cohortpatientview/update?UserId={5}",
                                                 Url,
                                                 Context,
                                                 Version,
                                                 ContractNumber,
                                                 patientId,
                                                 HeaderUserId));
            HttpClient client = GetHttpClient(cohortPatientUri);

            DataContractJsonSerializer jsonSer = new DataContractJsonSerializer(typeof(PutUpdateCohortPatientViewRequest));

            // use the serializer to write the object to a MemoryStream 
            MemoryStream ms = new MemoryStream();
            jsonSer.WriteObject(ms, request);
            ms.Position = 0;

            //use a Stream reader to construct the StringContent (Json) 
            StreamReader sr = new StreamReader(ms);

            StringContent theContent = new StringContent(sr.ReadToEnd(), System.Text.Encoding.UTF8, "application/json");

            //Post the data 
            var response = client.PutAsync(cohortPatientUri, theContent);
            var responseContent = response.Result.Content;

            string responseString = responseContent.ReadAsStringAsync().Result;
            PutUpdateCohortPatientViewResponse responseCohortPatientView = null;

            using (var msResponse = new MemoryStream(Encoding.Unicode.GetBytes(responseString)))
            {
                var serializer = new DataContractJsonSerializer(typeof(PutUpdateCohortPatientViewResponse));
                responseCohortPatientView = (PutUpdateCohortPatientViewResponse)serializer.ReadObject(msResponse);
            }

            return responseCohortPatientView;
        }

        private HttpClient GetHttpClient(Uri uri)
        {
            HttpClient client = new HttpClient();

            string userId = (HeaderUserId != string.Empty ? HeaderUserId : "000000000000000000000000");

            client.DefaultRequestHeaders.Host = uri.Host;
            client.DefaultRequestHeaders.Add("x-Phytel-UserID", userId);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }


    }
}
