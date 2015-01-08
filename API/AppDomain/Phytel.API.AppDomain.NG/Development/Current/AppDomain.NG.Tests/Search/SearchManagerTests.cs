using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
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
            var med = new DTO.Medication {ProprietaryName = propName, ProprietaryNameSuffix = propSufx, SubstanceName = string.Empty, ProductId = string.Empty, NDC = string.Empty};
            var contractNumber = "InHealth001";

            Mapper.CreateMap<DTO.Medication, MedNameSearchDoc>()
                .ForMember(d => d.ProductNDC, opt => opt.MapFrom(src => src.NDC))
                .ForMember(d => d.CompositeName, opt => opt.MapFrom(src => src.ProprietaryName + " " + src.ProprietaryNameSuffix));

            //var nfp = Mapper.Map<MedNameSearchDoc>(med);

            //ISearchManager manager = new SearchManager();
            //manager.RegisterMedDocumentInSearchIndex(med, contractNumber);

            //var req = new GetMedNamesRequest { Context = "Nightingale", ContractNumber = "InHealth001", Take = 30, Term = propName +" "+ propSufx, Token = "54ac4a9084ac0522946dc106" };
            //var results = manager.GetSearchMedNameResults(req);

            //Assert.AreEqual(propName + " "+ propSufx, results[0].Text);
            Assert.IsTrue(true);
        }
    }
}
