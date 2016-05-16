using Phytel.API.Common;
using Phytel.API.DataDomain.Patient.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.Engage.Integrations
{
    public static class DBScriptGenerator
    {
        private static string systemPath = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

        public static void SavePatients(List<HttpObjectResponse<PatientData>> data)
        {
            try
            {
                var complete = Path.Combine(systemPath, @"AtmosphereIntegration");

                StringBuilder sb = new StringBuilder();
                data.ForEach(r =>
                {
                    if (r.Code == System.Net.HttpStatusCode.OK)
                        sb.Append(@"{ _id: ObjectId('" + r.Body.Id + "')}" + Environment.NewLine);
                });

                (new FileInfo(complete)).Directory.Create();
                System.IO.File.WriteAllText(complete + @"\Patient.json", sb.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception("DBScriptGenerator:SavePatients():" + ex.Message);
            }
        }
    }
}
