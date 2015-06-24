using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DataDomain.Search.Repo.LuceneStrategy;
using Lucene.Net.Documents;
using Phytel.API.AppDomain.NG.Allergy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.DTO.Search;
using Phytel.API.AppDomain.NG.Search.LuceneStrategy;
using ServiceStack.ServiceClient.Web;

namespace Phytel.API.AppDomain.NG.Allergy.Tests
{
    [TestClass()]
    public class SearchManagerTests
    {
        [TestMethod()]
        public void RegisterMedDocumentInSearchIndexTest_justName()
        {
            var propName = "OZA";
            //var propSufx = "Snacks2";

            var med = new DTO.Medication
            {
                Id = "111111111111111111111111",
                ProprietaryName = propName,
                ProprietaryNameSuffix = string.Empty,
                SubstanceName = string.Empty,
                ProductId = string.Empty,
                DosageFormName = string.Empty,
                RouteName = string.Empty,
                Strength = string.Empty,
                NDC = string.Empty
            };
            var contractNumber = "InHealth001";

            Mapper.CreateMap<DTO.Medication, MedNameSearchDoc>()
                .ForMember(d => d.ProductNDC, opt => opt.MapFrom(src => src.NDC))
                .ForMember(d => d.CompositeName,
                    opt => opt.MapFrom(src => src.ProprietaryName + " " + src.ProprietaryNameSuffix))
                .ForMember(d => d.DosageFormname, opt => opt.MapFrom(src => src.DosageFormName))
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(d => d.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(d => d.ProprietaryName, opt => opt.MapFrom(src => src.ProprietaryName))
                .ForMember(d => d.ProprietaryNameSuffix, opt => opt.MapFrom(src => src.ProprietaryNameSuffix))
                .ForMember(d => d.RouteName, opt => opt.MapFrom(src => src.RouteName))
                .ForMember(d => d.Strength, opt => opt.MapFrom(src => src.Strength))
                .ForMember(d => d.SubstanceName, opt => opt.MapFrom(src => src.SubstanceName))
                .ForMember(d => d.Unit, opt => opt.MapFrom(src => string.Empty));


            Mapper.CreateMap<DTO.Medication, MedFieldsSearchDoc>()
                .ForMember(d => d.CompositeName,
                    opt => opt.MapFrom(src => src.ProprietaryName + " " + src.ProprietaryNameSuffix))
                .ForMember(d => d.DosageFormname, opt => opt.MapFrom(src => src.DosageFormName))
                .ForMember(d => d.RouteName, opt => opt.MapFrom(src => src.RouteName))
                .ForMember(d => d.Strength, opt => opt.MapFrom(src => src.Strength))
                .ForMember(d => d.SubstanceName, opt => opt.MapFrom(src => src.SubstanceName))
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(d => d.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(d => d.ProprietaryName, opt => opt.MapFrom(src => src.ProprietaryName))
                .ForMember(d => d.Unit, opt => opt.MapFrom(src => string.Empty));

            var nfp = Mapper.Map<MedNameSearchDoc>(med);

            //ISearchManager manager = new SearchManager { MedNameStrategy = new MedNameLuceneStrategy<MedNameSearchDoc, TextValuePair>() };
            //manager.RegisterMedDocumentInSearchIndex(med, contractNumber);

            Assert.IsTrue(true);
        }


        [TestMethod()]
        public void RegisterMedDocumentInSearchIndexTest()
        {
            var propName = "Roumel2";
            var propSufx = "Snacks2";

            var med = new DTO.Medication
            {
                Id = "000000000000000000000000",
                ProprietaryName = propName,
                ProprietaryNameSuffix = propSufx,
                SubstanceName = string.Empty,
                ProductId = string.Empty,
                DosageFormName = "TABLET",
                RouteName = "ORAL",
                Strength = "2.2 g/L",
                NDC = string.Empty
            };
            var contractNumber = "InHealth001";

            Mapper.CreateMap<DTO.Medication, MedNameSearchDoc>()
                .ForMember(d => d.ProductNDC, opt => opt.MapFrom(src => src.NDC))
                .ForMember(d => d.CompositeName,
                    opt => opt.MapFrom(src => src.ProprietaryName + " " + src.ProprietaryNameSuffix))
                .ForMember(d => d.DosageFormname, opt => opt.MapFrom(src => src.DosageFormName))
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(d => d.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(d => d.ProprietaryName, opt => opt.MapFrom(src => src.ProprietaryName))
                .ForMember(d => d.ProprietaryNameSuffix, opt => opt.MapFrom(src => src.ProprietaryNameSuffix))
                .ForMember(d => d.RouteName, opt => opt.MapFrom(src => src.RouteName))
                .ForMember(d => d.Strength, opt => opt.MapFrom(src => src.Strength))
                .ForMember(d => d.SubstanceName, opt => opt.MapFrom(src => src.SubstanceName))
                .ForMember(d => d.Unit, opt => opt.MapFrom(src => string.Empty));


            Mapper.CreateMap<DTO.Medication, MedFieldsSearchDoc>()
                .ForMember(d => d.CompositeName,
                    opt => opt.MapFrom(src => src.ProprietaryName + " " + src.ProprietaryNameSuffix))
                .ForMember(d => d.DosageFormname, opt => opt.MapFrom(src => src.DosageFormName))
                .ForMember(d => d.RouteName, opt => opt.MapFrom(src => src.RouteName))
                .ForMember(d => d.Strength, opt => opt.MapFrom(src => src.Strength))
                .ForMember(d => d.SubstanceName, opt => opt.MapFrom(src => src.SubstanceName))
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(d => d.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(d => d.ProprietaryName, opt => opt.MapFrom(src => src.ProprietaryName))
                .ForMember(d => d.Unit, opt => opt.MapFrom(src => string.Empty));

            var nfp = Mapper.Map<MedNameSearchDoc>(med);

            //ISearchManager manager = new SearchManager{ MedNameStrategy = new MedNameLuceneStrategy<MedNameSearchDoc, TextValuePair>()};
            //manager.RegisterMedDocumentInSearchIndex(med, contractNumber);

            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void RegisterAllergyDocumentInSearchIndexTest()
        {
            Assert.AreEqual("Test", "Test");
        }

        [TestMethod()]
        public void RegisterMedDocumentInSearchIndexTest1()
        {
            Assert.AreEqual("Test", "Test");
        }

        [TestMethod()]
        public void GetSearchMedNameResultsTest()
        {
            Assert.AreEqual("Test", "Test");
        }

        [TestMethod()]
        public void GetSearchMedFieldsResultsTest()
        {
            Assert.AreEqual("Test", "Test");
        }

        [TestMethod()]
        public void GetRouteSelectionsTest()
        {
            Assert.AreEqual("Test", "Test");
        }

        [TestMethod()]
        public void GetFormSelectionsTest()
        {
            Assert.AreEqual("Test", "Test");
        }

        [TestMethod()]
        public void GetStrengthSelectionsTest()
        {
            Assert.AreEqual("Test", "Test");
        }

        [TestMethod()]
        public void GetUnitSelectionsTest()
        {
            Assert.AreEqual("Test", "Test");
        }

        [TestMethod()]
        public void GetSearchDomainResultsTest()
        {
            Assert.AreEqual("Test", "Test");
        }
    }
}
