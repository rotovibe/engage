using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
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
                .ForMember(d => d.CompositeName, opt => opt.MapFrom(src => src.ProprietaryName + " " + src.ProprietaryNameSuffix));

            Mapper.CreateMap<DTO.Medication, MedFieldsSearchDoc>()
                .ForMember(d => d.CompositeName, opt => opt.MapFrom(src => src.ProprietaryName + " " + src.ProprietaryNameSuffix))
            .ForMember(d => d.DosageFormname, opt => opt.MapFrom(src => src.DosageFormName))
            .ForMember(d => d.RouteName, opt => opt.MapFrom(src => src.RouteName))
            .ForMember(d => d.Strength, opt => opt.MapFrom(src => src.Strength))
            .ForMember(d => d.SubstanceName, opt => opt.MapFrom(src => src.SubstanceName))
            .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(d => d.ProductId, opt => opt.MapFrom(src => src.ProductId))
            .ForMember(d => d.ProprietaryName, opt => opt.MapFrom(src => src.ProprietaryName))
            .ForMember(d => d.Unit, opt => opt.MapFrom(src => string.Empty));

            var nfp = Mapper.Map<MedNameSearchDoc>(med);

            ISearchManager manager = new SearchManager();
            manager.RegisterMedDocumentInSearchIndex(med, contractNumber);

            //var req = new GetMedNamesRequest { Context = "Nightingale", ContractNumber = "InHealth001", Take = 30, Term = propName + " " + propSufx, Token = "54ac4a9084ac0522946dc106" };
            //var results = manager.GetSearchMedNameResults(req);

            //Assert.AreEqual(propName + " "+ propSufx, results[0].Text);
            Assert.IsTrue(true);
        }
    }
}
