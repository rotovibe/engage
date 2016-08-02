using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.DTO.Search;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using AutoMapper;
using DataDomain.Medication.Repo;
using DataDomain.Search.Repo.LuceneStrategy;
using Phytel.API.DataDomain.Medication.DTO;
using Phytel.API.DataDomain.Search.DTO;

namespace Phytel.API.DataDomain.Medication.Test.Search
{
    [TestClass]
    public class Search_Index_Initialization_Test
    {
        //[TestMethod]
        //public void InitializeProductName()
        //{
        //    var listNames = new List<MedNameSearchDoc>();
            
        //    //GetProductInfoSql(listNames);
        //    GetProductInfo(listNames);
        //    var lucene = new MedNameLuceneStrategy<MedNameSearchDoc, TextValuePair>() { Contract = "InHealth001" };
        //    lucene.AddUpdateLuceneIndex(listNames);
        //}

        [TestMethod]
        public void InitializeMedicationMappingData()
        {
            var medlist = GetMedicationMongoList();

            var medGrouping = new Dictionary<MedicationKey, List<MedicationData>>();
            //var m = new MedicationKey{  Form = "", FullName="", Route ="", Strength="", SubstanceName=""};

            // create key registrations for each med
            InitializeKeyValueMedList(medlist, medGrouping);

            // hydrate key value list.
            HydrateMedGroupings(medlist, medGrouping);

            // create a flat list of medicationmapping
            var medmaps = HydrateMedMappingList(medGrouping);

            // insert medmaps into mongo
            var smap = InsertMedicationMappings(medmaps);
            
            var smapKeyList = new Dictionary<MedicationKey, string>();
            smap.ForEach(mk => { smapKeyList.Add(CreateMedicationMappingKey(mk), mk.Id); });

            foreach (MedicationKey k in medGrouping.Keys)
            {
                var tKey = smapKeyList[k];
                var med = medGrouping[k];
                med.ForEach(m => m.FamilyId = tKey);
            }

            var list = medGrouping.Values.SelectMany(x => x).ToList();

            var req = new PutMedicationMapDataRequest { Context = "NG", ContractNumber = "InHealth001", UserId = "5325c81f072ef705080d347e", Version=1 };
            var repo = MedicationRepositoryFactory.GetMedicationRepository(req, RepositoryType.Medication);

                repo.Update(list);

        }

        private List<MedicationMapData> InsertMedicationMappings(List<MedicationMapData> medmaps)
        {
            Mapper.CreateMap<MEMedicationMapping, DTO.MedicationMapData>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.RecordCreatedOn, opt => opt.MapFrom(src => src.RecordCreatedOn))
                .ForMember(dest => dest.RecordCreatedBy, opt => opt.MapFrom(src => src.RecordCreatedBy.ToString()))
                .ForMember(dest => dest.LastUpdatedOn, opt => opt.MapFrom(src => src.LastUpdatedOn))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy.ToString()));
            var req = new DomainRequest {Context = "NG", ContractNumber = "InHealth001"};
            var repo = MedicationRepositoryFactory.GetMedicationRepository(req, RepositoryType.MedicationMapping);
            var MedMp = (List<MedicationMapData>) repo.InsertAll(medmaps.ToList<object>());

            return MedMp;
        }

        private static List<MedicationMapData> HydrateMedMappingList(Dictionary<MedicationKey, List<MedicationData>> medGrouping)
        {
            var medmaps = new List<MedicationMapData>();
            foreach (MedicationKey key in medGrouping.Keys)
            {
                var medmap = new MedicationMapData
                {
                    Custom = false,
                    DeleteFlag = false,
                    Form = key.Form,
                    FullName = key.FullName,
                    Route = key.Route,
                    Strength = key.Strength,
                    Version = 1.0,
                    SubstanceName = key.SubstanceName,
                    Verified = true
                };

                if (!medmaps.Contains(medmap))
                {
                    medmaps.Add(medmap);
                }
            }
            return medmaps;
        }

        private void HydrateMedGroupings(List<MedicationData> medlist, Dictionary<MedicationKey, List<MedicationData>> medGrouping)
        {
            medlist.ForEach(md =>
            {
                var key = CreateMedicationKey(md);
                var list = medGrouping[key] as List<MedicationData>;
                if (!list.Contains(md))
                    list.Add(md);
            });
        }

        private void InitializeKeyValueMedList(List<MedicationData> medlist, Dictionary<MedicationKey, List<MedicationData>> medGrouping)
        {
            medlist.ForEach(md =>
            {
                var key = CreateMedicationKey(md);

                if (!medGrouping.ContainsKey(key))
                {
                    var list = new List<MedicationData>();
                    medGrouping.Add(key, list);
                }
            });
        }

        private MedicationKey CreateMedicationKey(MedicationData md)
        {
            var key = new MedicationKey
            {
                Form = md.Form,
                FullName = md.FullName,
                Route = md.Route,
                Strength = FormatStrengthDisplay(md.Strength, md.Unit),
                SubstanceName = md.SubstanceName
            };
            return key;
        }

        private MedicationKey CreateMedicationMappingKey(MedicationMapData md)
        {
            var key = new MedicationKey
            {
                Form = md.Form,
                FullName = md.FullName,
                Route = md.Route,
                Strength = md.Strength,
                SubstanceName = md.SubstanceName
            };
            return key;
        }

        public struct MedicationKey
        {
            public string FullName { get; set; }
            public string SubstanceName { get; set; }
            public string Route { get; set; }
            public string Form { get; set; }
            public string Strength { get; set; }
        }

        private void GetProductInfo<T>(List<T> listNames)
        {
            var result = GetMedicationMongoList();

            if (typeof (T) == typeof (MedNameSearchDoc))
            {
                HydrateMedNameDoc(listNames as List<MedNameSearchDoc>, result);
            }
            else
            {
                HydrateMedFieldsDoc(listNames as List<MedFieldsSearchDoc>, result);
            }
        }

        private string FormatStrengthDisplay(string strength, string unit)
        {
            var val = new StringBuilder();

            string[] strengthS = strength.Split(';');
            string[] unitS = unit.Split(';');

            for (int i = 0; i < strengthS.Length; i++)
            {
                if (i == strengthS.Length - 1)
                {
                    val.Append(strengthS[i] + " " + unitS[i]);
                }
                else
                {
                    val.Append(strengthS[i] + " " + unitS[i] + ";");
                }
            }

            return val.ToString();
        }

        private List<MedicationMapData> GetMedicationMappingMongoList()
        {
            Mapper.CreateMap<MEMedicationMapping, DTO.MedicationMapData>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.RecordCreatedOn, opt => opt.MapFrom(src => src.RecordCreatedOn))
                .ForMember(dest => dest.RecordCreatedBy, opt => opt.MapFrom(src => src.RecordCreatedBy.ToString()))
                .ForMember(dest => dest.LastUpdatedOn, opt => opt.MapFrom(src => src.LastUpdatedOn))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy.ToString()));

            var req = new GetAllMedicationsRequest {Context = "NG", ContractNumber = "InHealth001"};
            var repo = MedicationRepositoryFactory.GetMedicationRepository(req, RepositoryType.MedicationMapping);

            var result = repo.SelectAll().Cast<DTO.MedicationMapData>().Where(r => r.SubstanceName != null && r.FullName != null && r.Id != null).ToList<DTO.MedicationMapData>();
            return result;
        }

        private List<MedicationData> GetMedicationMongoList()
        {
            Mapper.CreateMap<MEMedication, DTO.MedicationData>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.RecordCreatedOn, opt => opt.MapFrom(src => src.RecordCreatedOn))
                .ForMember(dest => dest.RecordCreatedBy, opt => opt.MapFrom(src => src.RecordCreatedBy.ToString()))
                .ForMember(dest => dest.LastUpdatedOn, opt => opt.MapFrom(src => src.LastUpdatedOn))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy.ToString()));

            var req = new GetAllMedicationsRequest {Context = "NG", ContractNumber = "InHealth001"};
            var repo = MedicationRepositoryFactory.GetMedicationRepository(req, RepositoryType.Medication);

            var result = repo.SelectAll().Cast<DTO.MedicationData>().Where(r => r.SubstanceName != null && r.FullName != null && r.Id != null).ToList<DTO.MedicationData>();
            return result;
        }

        private static void HydrateMedFieldsDoc(List<MedFieldsSearchDoc> listNames, List<MedicationData> result)
        {
            result.ForEach(r =>
            {
                listNames.Add(
                    new MedFieldsSearchDoc
                    {
                        CompositeName = r.FullName,
                        Id = r.Id,
                        ProductId = r.ProductId,
                        ProprietaryName = r.ProprietaryName,
                        SubstanceName = r.SubstanceName,
                        DosageFormname = r.Form,
                        RouteName = r.Route,
                        Strength = r.Strength,
                        Unit = r.Unit
                    });
            });
        }

        private static void HydrateMedNameDoc(List<MedNameSearchDoc> listNames, List<MedicationData> result)
        {
            result.ForEach(r =>
            {
                listNames.Add(
                    new MedNameSearchDoc
                    {
                        CompositeName = r.FullName,
                        Id = r.Id,
                        ProductId = r.ProductId,
                        ProductNDC = r.NDC,
                        ProprietaryName = r.ProprietaryName,
                        ProprietaryNameSuffix = r.ProprietaryNameSuffix,
                        SubstanceName = r.SubstanceName,
                        RouteName = r.Route,
                        DosageFormname = r.Form,
                        Strength = r.Strength,
                        Unit = r.Unit
                    });
            });
        }

        private static void GetProductInfoSql(List<MedNameSearchDoc> listNames)
        {
            var connectionString = "Data Source=localhost;Initial Catalog=AllergiesAndMeds;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (
                SqlCommand command =
                    new SqlCommand(
                        @"select * FROM AllergiesAndMeds.dbo.products WHERE PRODUCTTYPENAME not in ('VACCINE', 'PLASMA DERIVATIVE', 'NON-STANDARDIZED ALLERGENIC', 'STANDARDIZED ALLERGENIC', 'CELLULAR THERAPY')",
                        connection))
            {
                connection.Open();
                using (SqlDataReader rdr = command.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        listNames.Add(new MedNameSearchDoc
                        {
                            ProductId = rdr["PRODUCTID"].ToString().Trim().ToUpper().Replace("\"", ""),
                            ProductNDC = rdr["PRODUCTNDC"].ToString().Trim().ToUpper().Replace("\"", ""),
                            CompositeName = rdr["PROPRIETARYNAME"].ToString().Trim().ToUpper().Replace("\"", "") + " " +
                                            rdr["PROPRIETARYNAMESUFFIX"].ToString().Trim().ToUpper().Replace("\"", ""),
                            ProprietaryName = rdr["PROPRIETARYNAME"].ToString().Trim().ToUpper().Replace("\"", ""),
                            ProprietaryNameSuffix = rdr["PROPRIETARYNAMESUFFIX"].ToString().Trim().ToUpper().Replace("\"", ""),
                            SubstanceName = rdr["SUBSTANCENAME"].ToString().Trim().ToUpper().Replace("\"", "")
                        });
                    }
                }
            }
        }


        private static void GetProductMedFieldsSql(List<MedFieldsSearchDoc> listNames)
        {
            var connectionString = "Data Source=localhost;Initial Catalog=AllergiesAndMeds;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(@"SELECT 
                                                            PRODUCTID, 
                                                            PROPRIETARYNAME, 
                                                            PROPRIETARYNAMESUFFIX, 
                                                            SUBSTANCENAME,  
                                                            ROUTENAME, 
                                                            DOSAGEFORMNAME, 
                                                            ACTIVE_NUMERATOR_STRENGTH,  
                                                            ACTIVE_INGRED_UNIT
                                                        FROM [AllergiesAndMeds].[dbo].[products]", connection))
            {
                connection.Open();
                using (SqlDataReader rdr = command.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        listNames.Add(new MedFieldsSearchDoc
                        {
                            ProductId = rdr["PRODUCTID"].ToString().Trim().ToUpper(),
                            CompositeName = rdr["PROPRIETARYNAME"].ToString().Trim().ToUpper().Replace("\"", "") + " " +
                                            rdr["PROPRIETARYNAMESUFFIX"].ToString().Trim().ToUpper().Replace("\"", ""),
                            ProprietaryName = rdr["PROPRIETARYNAME"].ToString().Trim().ToUpper().Replace("\"", ""),
                            SubstanceName = rdr["SUBSTANCENAME"].ToString().Trim().ToUpper().Replace("\"", ""),
                            RouteName = rdr["ROUTENAME"].ToString().Trim().ToUpper().Replace("\"", ""),
                            DosageFormname = rdr["DOSAGEFORMNAME"].ToString().Trim().ToUpper().Replace("\"", ""),
                            Strength = rdr["ACTIVE_NUMERATOR_STRENGTH"].ToString().Trim().ToUpper(),
                            Unit = rdr["ACTIVE_INGRED_UNIT"].ToString().Trim()
                        });
                    }
                }
            }
        }
    }
}
