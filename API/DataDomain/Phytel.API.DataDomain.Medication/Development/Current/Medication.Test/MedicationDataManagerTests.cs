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
            var dm = new MedicationDataManager(repo);

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
                          FROM [AllergiesAndMeds].[dbo].[products_Array_formatted]",
                        connection))
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var rec = new DTO.MedicationData
                        {
                            ProductId = reader["PRODUCTID"].ToString(),
                            NDC = reader["PRODUCTNDC"].ToString(),
                            ProprietaryName = reader["PROPRIETARYNAME"].ToString(),
                            ProprietaryNameSuffix = reader["PROPRIETARYNAMESUFFIX"].ToString(),
                            StartDate = formatDate(reader["STARTMARKETINGDATE"].ToString()),
                            SubstanceName = GetList(reader["SUBSTANCENAME"].ToString()),
                            PharmClass = GetList(reader["PHARM_CLASSES"].ToString()),
                            Route = GetList(reader["ROUTENAME"].ToString()),
                            Form = reader["DOSAGEFORMNAME"].ToString(),
                            Unit = GetList(reader["ACTIVE_INGRED_UNIT"].ToString()),
                            Strength = GetList(reader["ACTIVE_NUMERATOR_STRENGTH"].ToString()),
                            RecordCreatedBy = "000000000000000000000000",
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

        private List<string> GetList(string p)
        {
            p = p.Replace("[\"", "");
            p = p.Replace("\"]", "");
            p = p.Replace("\"", "");
            var split = p.Split(',');

            var list = new List<string>();
            split.ToList().ForEach(list.Add);
            if (split.Length > 1)
            {
                var test = "";
            }
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
