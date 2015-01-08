using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.DTO.Search;
using Phytel.API.AppDomain.NG.Search.LuceneStrategy;
using System.Data.SqlClient;

namespace Phytel.API.AppDomain.NG.Test.Search
{
    [TestClass]
    public class Search_Index_Initialization_Test
    {
        [TestMethod]
        public void InitializeProductName()
        {
            var listNames = new List<MedNameSearchDoc>();
            var connectionString = "Data Source=localhost;Initial Catalog=AllergiesAndMeds;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(@"select * FROM AllergiesAndMeds.dbo.products WHERE PRODUCTTYPENAME not in ('VACCINE', 'PLASMA DERIVATIVE', 'NON-STANDARDIZED ALLERGENIC', 'STANDARDIZED ALLERGENIC', 'CELLULAR THERAPY')", connection))
            {
                connection.Open();
                using (SqlDataReader rdr = command.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        listNames.Add(new MedNameSearchDoc
                        {
                            ProductId = rdr["PRODUCTID"].ToString().Trim().ToUpper().Replace("\"", "").Replace(",", ""),
                            ProductNDC = rdr["PRODUCTNDC"].ToString().Trim().ToUpper().Replace("\"", "").Replace(",", ""),
                            CompositeName = rdr["PROPRIETARYNAME"].ToString().Trim().ToUpper().Replace("\"", "").Replace(",", "") + " " +
                                rdr["PROPRIETARYNAMESUFFIX"].ToString().Trim().ToUpper().Replace("\"", "").Replace(",", ""),
                            ProprietaryName = rdr["PROPRIETARYNAME"].ToString().Trim().ToUpper().Replace("\"", "").Replace(",", ""),
                            ProprietaryNameSuffix = rdr["PROPRIETARYNAMESUFFIX"].ToString().Trim().ToUpper().Replace("\"", "").Replace(",", ""),
                            SubstanceName = rdr["SUBSTANCENAME"].ToString().Trim().ToUpper().Replace("\"", "").Replace(",", "")
                        });
                    }
                }
            }

            var lucene = new MedNameLuceneStrategy<MedNameSearchDoc, TextValuePair>() { Contract = "InHealth001" };
            lucene.AddUpdateLuceneIndex(listNames);
        }

        [TestMethod]
        public void InitializeProductFields()
        {
            var listNames = new List<MedFieldsSearchDoc>();
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
                            CompositeName = rdr["PROPRIETARYNAME"].ToString().Trim().ToUpper().Replace("\"", "").Replace(",", "") + " " +
                                rdr["PROPRIETARYNAMESUFFIX"].ToString().Trim().ToUpper().Replace("\"", "").Replace(",", ""),
                            ProprietaryName = rdr["PROPRIETARYNAME"].ToString().Trim().ToUpper().Replace("\"", "").Replace(",", ""),
                            SubstanceName = rdr["SUBSTANCENAME"].ToString().Trim().ToUpper().Replace("\"", ""),
                            RouteName = rdr["ROUTENAME"].ToString().Trim().ToUpper().Replace("\"", ""),
                            DosageFormname = rdr["DOSAGEFORMNAME"].ToString().Trim().ToUpper().Replace("\"", ""),
                            Strength = rdr["ACTIVE_NUMERATOR_STRENGTH"].ToString().Trim().ToUpper(),
                            Unit = rdr["ACTIVE_INGRED_UNIT"].ToString().Trim()
                        });
                    }
                }
            }

            var lucene = new MedFieldsLuceneStrategy<MedFieldsSearchDoc, MedFieldsSearchDoc>() { Contract = "InHealth001" };
            lucene.AddUpdateLuceneIndex(listNames);
        }
    }
}
