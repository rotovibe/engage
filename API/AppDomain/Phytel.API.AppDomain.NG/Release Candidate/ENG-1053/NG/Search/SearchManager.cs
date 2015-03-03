using System;
using Phytel.API.AppDomain.NG.DTO.Search;
using Phytel.API.AppDomain.NG.Search;
using Phytel.API.AppDomain.NG.Search.LuceneStrategy;
using Phytel.API.Common.CustomObject;
using ServiceStack.ServiceClient.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Phytel.API.AppDomain.NG.DTO;

namespace Phytel.API.AppDomain.NG.Allergy
{
    public class SearchManager : ManagerBase, ISearchManager
    {
        public ISearchUtil SearchUtil { get; set; }
        public ISearchEndpointUtil EndpointUtil { get; set; }
        public IMedNameLuceneStrategy<MedNameSearchDoc, TextValuePair> MedNameStrategy { get; set; }

        public void RegisterAllergyDocumentInSearchIndex(DTO.Allergy allergy,string contractNumber)
        {
            try
            {
                var np =  AutoMapper.Mapper.Map<IdNamePair>(allergy);
                new AllergyLuceneStrategy<IdNamePair, IdNamePair> { Contract = contractNumber }.AddUpdateLuceneIndex(np);
            }
            catch (Exception ex)
            {
                throw new WebServiceException("AD:RegisterAllergyDocumentInSearchIndex()::" + ex.Message, ex.InnerException);
            }
        }

        public void RegisterMedDocumentInSearchIndex(DTO.Medication med, string contractNumber)
        {
            try
            {
                var nfp = AutoMapper.Mapper.Map<MedNameSearchDoc>(med);
                MedNameStrategy.AddUpdateLuceneIndex(nfp);
            }
            catch (Exception ex)
            {
                throw new WebServiceException("AD:RegisterMedDocumentInSearchIndex()::" + ex.Message, ex.InnerException);
            }
        }


        public List<TextValuePair> GetSearchMedNameResults(GetMedNamesRequest request)
        {
            try
            {
                var matches = MedNameStrategy.Search(request.Term);
                matches.All(x => { x.Text = x.Text.Trim(); return true; });
                var groupby = matches.GroupBy(a => a.Text).Select(s => s.First()).ToList();
                var result = groupby;
                return result;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:GetSearchDomainResults()::" + ex.Message, ex.InnerException);
            }
        }

        public MedFieldsLists GetSearchMedFieldsResults(GetMedFieldsRequest request)
        {
            try
            {
                var lists = new MedFieldsLists();
                var matches = EndpointUtil.GetMedicationMapsByName(request, request.UserId);
                var fMatches = SearchUtil.FilterFieldResultsByParams(request, matches);

                // break out into seperate lists here.
                lists.RouteList = GetRouteSelections(fMatches);
                lists.FormList = GetFormSelections(fMatches);
                lists.StrengthList = GetStrengthSelections(fMatches);
                return lists;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:GetSearchDomainResults()::" + ex.Message, ex.InnerException);
            }
        }

        public List<TextValuePair> GetRouteSelections(List<MedicationMap> matches)
        {
            try
            {
                var vals = new List<TextValuePair>();
                matches.ForEach(
                    m =>
                    {
                        var pair = new TextValuePair
                        {
                            Text = m.Route,
                            Value = m.Route
                        };

                        if (m.Route != null)
                        {
                            var rec = vals.Find(f => f.Value == m.Route);
                            if (rec == null)
                                vals.Add(pair);
                        }
                    });

                SortSelections(vals);
                return vals;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:GetRouteSelections()::" + ex.Message, ex.InnerException);
            }
        }

        public List<TextValuePair> GetFormSelections(List<MedicationMap> matches)
        {
            try
            {
                var vals = new List<TextValuePair>();
                matches.ForEach(
                    m =>
                    {
                        var pair = new TextValuePair{ Text = m.Form, Value = m.Form};

                        if (m.Form != null)
                        {
                            var rec = vals.Find(f => f.Value == m.Form);
                            if (rec == null)
                                vals.Add(pair);
                        }
                    });

                SortSelections(vals);
                return vals;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:GetFormSelections()::" + ex.Message, ex.InnerException);
            }
        }

        public List<TextValuePair> GetStrengthSelections(List<MedicationMap> matches)
        {
            try
            {
                var vals = new List<TextValuePair>();
                matches.ForEach(
                    m =>
                    {
                        var val = m.Strength; //FormatStrengthDisplay(m.Strength, m.Unit);
                        var pair = new TextValuePair
                        {
                            Text = val,
                            Value = val
                        };

                        if (m.Strength != null)
                        {
                            var rec = vals.Find(f => f.Text == val);
                            if (rec == null)
                                vals.Add(pair);
                        }
                    });
                SortSelections(vals);
                return vals;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:GetStrengthSelections()::" + ex.Message, ex.InnerException);
            }
        }

        private static void SortSelections(List<TextValuePair> vals)
        {
            vals.Sort((emp1, emp2) => emp1.Text.CompareTo(emp2.Text));
        }

        //private string FormatStrengthDisplay(string strength, string unit)
        //{
        //    try
        //    {
        //        if (unit.Length == 0) return strength.Trim();

        //        var val = new StringBuilder();

        //        string[] strengthS = strength.Split(';');
        //        string[] unitS = unit.Split(';');

        //        for (int i = 0; i < strengthS.Length; i++)
        //        {
        //            if (i == strengthS.Length - 1)
        //            {
        //                val.Append((strengthS[i] + " " + unitS[i]).Trim());
        //            }
        //            else
        //            {
        //                val.Append((strengthS[i] + " " + unitS[i] + ";").Trim());
        //            }
        //        }

        //        return val.ToString();
        //    }
        //    catch (WebServiceException ex)
        //    {
        //        throw new WebServiceException("AD:GetStrengthSelections()::" + ex.Message, ex.InnerException);
        //    }
        //}

        //public List<TextValuePair> GetUnitSelections(List<MedicationMap> matches)
        //{
        //    try
        //    {
        //        var vals = new List<TextValuePair>();
        //        matches.ForEach(
        //            m =>
        //            {
        //                var pair = new TextValuePair
        //                {
        //                    Text = m.Unit,
        //                    Value = m.Unit
        //                };

        //                var rec = vals.Find(f => f.Text == m.Unit);
        //                if (rec == null)
        //                    vals.Add(pair);
        //            });
        //        return vals;
        //    }
        //    catch (WebServiceException ex)
        //    {
        //        throw new WebServiceException("AD:GetStrengthSelections()::" + ex.Message, ex.InnerException);
        //    }
        //}

        public List<IdNamePair> GetSearchDomainResults(GetSearchResultsRequest request)
        {
            try
            {
                // create a switch to determine which lucene strat to use
                var result = new AllergyLuceneStrategy<IdNamePair, IdNamePair> { Contract = request.ContractNumber }.SearchComplex(request.SearchTerm, "Name");
                return result;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:GetSearchDomainResults()::" + ex.Message, ex.InnerException);
            }
        }


    }
}
