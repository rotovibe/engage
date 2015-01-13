using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.DTO.Search;
using Phytel.API.AppDomain.NG.Search.LuceneStrategy;
using System.Data.SqlClient;
using System.Linq;
using AutoMapper;
using DataDomain.Medication.Repo;
using Phytel.API.DataDomain.Medication.DTO;

namespace Phytel.API.DataDomain.Medication.Test.Search
{
    [TestClass]
    public class Search_Index_Initialization_Test
    {
        [TestMethod]
        public void InitializeProductName()
        {
            var listNames = new List<MedNameSearchDoc>();
            
            //GetProductInfoSql(listNames);
            GetProductInfoMongo(listNames);
            var lucene = new MedNameLuceneStrategy<MedNameSearchDoc, TextValuePair>() { Contract = "InHealth001" };
            lucene.AddUpdateLuceneIndex(listNames);
        }

        private void GetProductInfoMongo<T>(List<T> listNames)
        {
            Mapper.CreateMap<MEMedication, DTO.MedicationData>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.RecordCreatedOn, opt => opt.MapFrom(src => src.RecordCreatedOn))
                .ForMember(dest => dest.RecordCreatedBy, opt => opt.MapFrom(src => src.RecordCreatedBy.ToString()))
                .ForMember(dest => dest.LastUpdatedOn, opt => opt.MapFrom(src => src.LastUpdatedOn))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy.ToString()));

            var req = new GetAllMedicationsRequest { Context = "NG", ContractNumber="InHealth001"};
            var repo = MedicationRepositoryFactory.GetMedicationRepository(req, RepositoryType.Medication);

            var result = repo.SelectAll().Cast<DTO.MedicationData>().ToList<DTO.MedicationData>();

            if (typeof (T) == typeof (MedNameSearchDoc))
            {
                HydrateMedNameDoc(listNames as List<MedNameSearchDoc>, result);
            }
            else
            {
                HydrateMedFieldsDoc(listNames as List<MedFieldsSearchDoc>, result);
            }
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
                        SubstanceName = r.SubstanceName
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

        [TestMethod]
        public void InitializeProductFields()
        {
            var listNames = new List<MedFieldsSearchDoc>();
            //GetProductMedFieldsSql(listNames);
            GetProductInfoMongo(listNames);

            var lucene = new MedFieldsLuceneStrategy<MedFieldsSearchDoc, MedFieldsSearchDoc>() { Contract = "InHealth001" };
            lucene.AddUpdateLuceneIndex(listNames);
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
