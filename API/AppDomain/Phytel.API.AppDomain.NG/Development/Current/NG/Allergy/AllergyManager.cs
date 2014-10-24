using AutoMapper;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using Lucene.Net.Store;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.DTO;
using Phytel.API.AppDomain.NG.DTO.Search;
using Phytel.API.AppDomain.NG.Search;
using Phytel.API.DataDomain.Allergy.DTO;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using System;
using System.Collections.Generic;
using System.Configuration;
using Phytel.API.Common;

namespace Phytel.API.AppDomain.NG.Allergy
{
    public class AllergyManager : ManagerBase, IAllergyManager
    {
        public IAllergyEndpointUtil EndpointUtil { get; set; }

        public List<DTO.Allergy> GetAllergies(GetAllergiesRequest request)
        {
            try
            {
                List<DTO.Allergy> result = new List<DTO.Allergy>();
                var algy = EndpointUtil.GetAllergies(request);
                algy.ForEach(a => result.Add(Mapper.Map<DTO.Allergy>(a)));
                IndexResultSet(result);
                return result;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:GetAllergies()::" + ex.Message, ex.InnerException);
            }
        }

        public void IndexResultSet(List<DTO.Allergy> result)
        {
            try
            {
                var searchDocs = new List<SearchedItem>();
                result.ForEach(a => searchDocs.Add(Mapper.Map<SearchedItem>(a)));
                LuceneManager.AddUpdateLuceneIndex(searchDocs);
            }
            catch (Exception ex)
            {
                throw new Exception("AD:IndexResultSet()::" + ex.Message, ex.InnerException);
            }
        }
    }
}
