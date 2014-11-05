﻿using System;
using Phytel.API.AppDomain.NG.DTO.Meds;
using Phytel.API.AppDomain.NG.DTO.Search;
using Phytel.API.AppDomain.NG.Search;
using Phytel.API.AppDomain.NG.Search.LuceneStrategy;
using Phytel.API.Common.CustomObject;
using ServiceStack.ServiceClient.Web;
using System.Collections.Generic;
using System.Linq;

namespace Phytel.API.AppDomain.NG.Allergy
{
    public class SearchManager : ManagerBase, ISearchManager
    {
        public ISearchUtil SearchUtil { get; set; }

        public void RegisterDocumentInSearchIndex(DTO.Allergy allergy,string contractNumber)
        {
            try
            {
                var np =  AutoMapper.Mapper.Map<IdNamePair>(allergy);
                new AllergyLuceneStrategy<IdNamePair, IdNamePair> { Contract = contractNumber }.AddUpdateLuceneIndex(np);
            }
            catch (Exception ex)
            {
                throw new WebServiceException("AD:RegisterDocumentInSearchIndex()::" + ex.Message, ex.InnerException);
            }
        }

        public void RegisterMedDocumentInSearchIndex(DTO.Allergy allergy)
        {
            try
            {
                //var np = AutoMapper.Mapper.Map<IdNamePair>(allergy);
                //new MedFieldsLuceneStrategy<Medication>().AddUpdateLuceneIndex(np);
            }
            catch (Exception ex)
            {
                throw new WebServiceException("AD:RegisterDocumentInSearchIndex()::" + ex.Message, ex.InnerException);
            }
        }


        public List<TextValuePair> GetSearchMedNameResults(GetMedNamesRequest request)
        {
            try
            {
                var matches = new MedNameLuceneStrategy<MedNameSearchDoc, TextValuePair> { Contract = request.ContractNumber }.Search(request.Term);
                matches.All(x => { x.Text = x.Text.Trim(); return true; });
                var groupby = matches.GroupBy(a => a.Text).Select(s => s.First()).ToList();
                //var result = groupby.OrderBy(g => g.Text.Length).ToList();
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
                var matches = new MedFieldsLuceneStrategy<MedFieldsSearchDoc, MedFieldsSearchDoc>{ Contract = request.ContractNumber}.Search(request.Name, "CompositeName");

                matches = SearchUtil.FilterFieldResultsByParams(request, matches);

                // break out into seperate lists here.
                lists.RouteList = GetRouteSelections(matches);
                lists.FormList = GetFormSelections(matches);
                lists.StrengthList = GetStrengthSelections(matches);
                lists.UnitsList = GetUnitSelections(matches);
                return lists;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:GetSearchDomainResults()::" + ex.Message, ex.InnerException);
            }
        }

        public List<TextValuePair> GetRouteSelections(List<MedFieldsSearchDoc> matches)
        {
            try
            {
                var vals = new List<TextValuePair>();
                matches.ForEach(
                    m =>
                    {
                        var pair = new TextValuePair
                        {
                            Text = m.RouteName,
                            Value = m.RouteName
                        };

                        var rec = vals.Find(f => f.Value == m.RouteName);
                        if (rec == null)
                            vals.Add(pair);
                    });
                //matches.GroupBy(b => b.RouteName).Select(s => s.First()).ToList().ForEach(m => vals.Add(new TextValuePair {Text = m.RouteName, Value = m.RouteName}));
                return vals;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:GetRouteSelections()::" + ex.Message, ex.InnerException);
            }
        }

        public List<TextValuePair> GetFormSelections(List<MedFieldsSearchDoc> matches)
        {
            try
            {
                var vals = new List<TextValuePair>();
                matches.ForEach(
                    m =>
                    {
                        var pair = new TextValuePair
                        {
                            Text = m.DosageFormname,
                            Value = m.DosageFormname
                        };

                        var rec = vals.Find(f => f.Value == m.DosageFormname);
                        if (rec == null)
                            vals.Add(pair);
                    });

                //matches.GroupBy(b => b.DosageFormname).Select(s => s.First()).ToList().ForEach(m => vals.Add(new TextValuePair { Text = m.DosageFormname, Value = m.DosageFormname }));
                return vals;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:GetFormSelections()::" + ex.Message, ex.InnerException);
            }
        }

        public List<TextValuePair> GetStrengthSelections(List<MedFieldsSearchDoc> matches)
        {
            try
            {
                var vals = new List<TextValuePair>();
                matches.ForEach(
                    m =>
                    {
                        var pair = new TextValuePair
                        {
                            Text = m.Strength + " " + m.Unit,
                            Value = m.Strength
                        };

                        var rec = vals.Find(f => f.Text == m.Strength + " " + m.Unit);
                        if (rec == null)
                            vals.Add(pair);
                    });
                return vals;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:GetStrengthSelections()::" + ex.Message, ex.InnerException);
            }
        }

        public List<TextValuePair> GetUnitSelections(List<MedFieldsSearchDoc> matches)
        {
            try
            {
                var vals = new List<TextValuePair>();
                matches.ForEach(
                    m =>
                    {
                        var pair = new TextValuePair
                        {
                            Text = m.Unit,
                            Value = m.Unit
                        };

                        var rec = vals.Find(f => f.Text == m.Unit);
                        if (rec == null)
                            vals.Add(pair);
                    });
                return vals;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:GetStrengthSelections()::" + ex.Message, ex.InnerException);
            }
        }

        public List<IdNamePair> GetSearchDomainResults(GetSearchResultsRequest request)
        {
            try
            {
                // create a switch to determine which lucene strat to use
                var result = new AllergyLuceneStrategy<IdNamePair, IdNamePair> { Contract = request.ContractNumber }.SearchComplex(request.SearchTerm, "Name");
                //var result = new AllergyLuceneStrategy<IdNamePair>().Search(request.SearchTerm);
                return result;
            }
            catch (WebServiceException ex)
            {
                throw new WebServiceException("AD:GetSearchDomainResults()::" + ex.Message, ex.InnerException);
            }
        }


    }
}
