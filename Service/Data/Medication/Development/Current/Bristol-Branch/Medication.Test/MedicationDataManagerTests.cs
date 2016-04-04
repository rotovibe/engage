using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataDomain.Medication.Repo;
using Phytel.API.DataDomain.Medication;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.DataDomain.Medication.DTO;
namespace Phytel.API.DataDomain.Medication.Tests
{
    [TestClass()]
    public class MedicationDataManagerTests
    {
        [TestMethod()]
        public void BulkInsertMedicationsTest()
        {
            PutBulkInsertMedicationsRequest request = new PutBulkInsertMedicationsRequest
            { 
                Context = "NG",
                ContractNumber = "InHealth001",
                UserId = "1234",
                Version = 1.0,
                Medications  = GetMedDtoList()
            };
            var repo = MedicationRepositoryFactory.GetMedicationRepository(request, RepositoryType.Medication);
//            var dm = new MedicationDataManager(repo);
            var dm = new MedicationDataManager();

            dm.BulkInsertMedications(request.Medications, request);
        }

        private List<DTO.MedicationData> GetMedDtoList()
        {
            var medList = new List<DTO.MedicationData>();
            var connectionString = "Data Source=localhost;Initial Catalog=AllergiesAndMeds;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (
                SqlCommand command =
                    new SqlCommand(
                        @"SELECT [PRODUCTID]
                          ,[PRODUCTNDC]
                          ,[PROPRIETARYNAME]
                          ,[PROPRIETARYNAMESUFFIX]
                          ,[STARTMARKETINGDATE]
                          ,[SUBSTANCENAME]
                          ,[PHARM_CLASSES]
                          ,[ROUTENAME]
                          ,[DOSAGEFORMNAME]
                          ,[ACTIVE_INGRED_UNIT]
                          ,[ACTIVE_NUMERATOR_STRENGTH]
                        FROM [AllergiesAndMeds].[dbo].[products]  ",
                        connection))
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var rec = new DTO.MedicationData
                        {
                            ProductId = reader["PRODUCTID"].ToString().Trim(),
                            NDC = reader["PRODUCTNDC"].ToString().Trim(),
                            FullName = GetFullName(reader["PROPRIETARYNAME"].ToString().Trim() , reader["PROPRIETARYNAMESUFFIX"].ToString().Trim()),
                            ProprietaryName = reader["PROPRIETARYNAME"].ToString().Trim().Replace("\"", "").ToUpper(),
                            ProprietaryNameSuffix = reader["PROPRIETARYNAMESUFFIX"].ToString().Trim().Replace("\"", "").ToUpper(),
                            StartDate = formatDate(reader["STARTMARKETINGDATE"].ToString().Trim()),
                            SubstanceName = reader["SUBSTANCENAME"].ToString().Trim().ToUpper(), //GetList(reader["SUBSTANCENAME"].ToString().Trim()),
                            PharmClass = GetList(reader["PHARM_CLASSES"].ToString().Trim()),
                            Route = reader["ROUTENAME"].ToString().Trim(), //GetList(reader["ROUTENAME"].ToString().Trim()),
                            Form = reader["DOSAGEFORMNAME"].ToString().Trim(),
                            Unit = reader["ACTIVE_INGRED_UNIT"].ToString().Trim(),//GetList(reader["ACTIVE_INGRED_UNIT"].ToString().Trim()),
                            Strength = reader["ACTIVE_NUMERATOR_STRENGTH"].ToString().Trim(), //GetList(reader["ACTIVE_NUMERATOR_STRENGTH"].ToString().Trim()),
                            RecordCreatedBy = "5368ff2ad4332316288f3e3e",
                            RecordCreatedOn = System.DateTime.UtcNow,
                            DeleteFlag = false,
                            Version = 1
                        };

                        medList.Add(rec);
                    }
                }
            }

            return medList;
        }

        private string GetFullName(string propName, string suffix)
        {
            var fpN = propName.Replace("\"", "");
            var sfx = suffix.Replace("\"", "");
            var name = fpN;
            if (suffix.Length > 0)
                name = fpN + " " + sfx;
            return name.ToUpper();
        }

        private List<string> GetList(string p)
        {
            if (p.Equals("[\"\"]")) return null;

            p = p.Replace("[\"", "");
            p = p.Replace("\"]", "");
            p = p.Replace("\"", "");
            var split = p.Split(',');
            split.Select(i => i.Trim());

            var list = new List<string>();
            split.ToList().ForEach(list.Add);

            return list;
        }

        private DateTime formatDate(string val)
        {
            var d = new DateTime();
            if (val.Length <= 0) return d;
            
            var date = DateTime.ParseExact(val,
                "yyyyMMdd",
                CultureInfo.InvariantCulture);

            //2014-11-04
            d = date.ToUniversalTime();

            return d;
        }
    }
}
