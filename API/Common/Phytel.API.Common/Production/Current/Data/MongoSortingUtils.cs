using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Phytel.API.Interface;
using System.Reflection;

namespace Phytel.API.Common.Data
{
    public static class MongoSortingUtils
    {
        public static MongoCursor ApplySkipTake(MongoCursor mongoCursor,  ISortableRequest dataRequest){
            int take = MongoSortingUtils.GetTake(dataRequest);
            if (take > 0)
            {
                mongoCursor = mongoCursor.SetLimit(take);
            }
            int skip = MongoSortingUtils.GetSkip(dataRequest);
            if (skip > 0)
            {
                mongoCursor = mongoCursor.SetSkip(skip);
            }
            return mongoCursor;
        }

        private static int GetSkip(ISortableRequest request)
        {
            int skip = 0;
            if (request.Skip != null && request.Skip >= 0)
            {
                skip = (int)request.Skip;
            }
            return skip;
        }

        private static int GetTake(ISortableRequest request)
        {
            int take = 0;
            if (request.Take != null && request.Take >= 0)
            {
                take = (int)request.Take;
            }
            return take;
        }

        private static string GetSortByProperty(string sort)
        {
            //expecting only one sort field for now:
            string sortByProperty = null;
            if (sort != null)
            {
                sortByProperty = sort;
                if (sort.StartsWith("+") || sort.StartsWith("-"))
                {
                    sortByProperty = sort.Substring(1);
                }
            }
            return sortByProperty;
        }

        private static string GetSortDirection( string sort ){
            string sortDirection = "+"; //default to asc sort
            if (sort != null)
            {
                //expecting only one sort field for now:
                if (sort.StartsWith("-"))
                {
                    sortDirection = "-";                    
                }
                else if (sort.StartsWith("+"))
                {
                    sortDirection = "+";                    
                }                
            }
            return sortDirection;
        }

        public static SortByBuilder GetSortByBuilder(string sort, Type entityType)
        {
            //sorting logic
            //sort - expecting a property name of the mongo entity, prefixed by sort direction +/- (= ascending / descending ) + is default. 
            //      for multiple sort fields ',' is the separator between sort properties expressions <+/-><Field1>,<+/-><Field2>, ... 
            //  examples: for entity METoDo: 
            //    +DueDate 
            //    -DueDate,-title  
            //entityType - expecting the mongo entity type. for example: typeof(METoDo)

            string[] sortExpressions = sort != null ? sort.Split(',') : new string[] {};
            SortByBuilder sortBy = new SortByBuilder();
            string sortByProperty;
            string sortDirection;
            string dbSortByField;
            if( sortExpressions.Length > 0 ){
                foreach ( string sortExpression in sortExpressions ){
                    sortByProperty = GetSortByProperty(sortExpression);
                    sortDirection = GetSortDirection(sortExpression);
                    //get a string of the mongo property db name to sort by
                    dbSortByField = GetSortByDbFieldName(sortByProperty, entityType);
                    sortBy = AddSorting( sortBy, sortDirection, dbSortByField);
                }
            }
            else{
                dbSortByField = GetDefaultSortByDbFieldName(entityType);
                sortDirection = GetSortDirection(dbSortByField); // +/-
                dbSortByField = GetSortByProperty(dbSortByField); // cleaned database prop 
                sortBy = AddSorting(sortBy, sortDirection, dbSortByField);
            }            
            return sortBy;
        }

        private static string GetDefaultSortByDbFieldName(Type entityType)
        {
            string dbSortBy = null;
            FieldInfo defaultSort = entityType.GetField("DefaultSort");
            if (defaultSort == null)
            {
                throw new Exception("The entity: " + entityType.Name + " must have a property named: DefaultSort.");
            }
            dbSortBy = (string)defaultSort.GetRawConstantValue();
            return dbSortBy;
        }

        private static SortByBuilder AddSorting(SortByBuilder sortBy, string sortDirection, string dbSortField)
        {
            //SortByBuilder sortBy;
            switch (sortDirection)
            {
                case "+":
                    {
                        sortBy = sortBy.Ascending(dbSortField);
                    }
                    break;
                case "-":
                    {
                        sortBy = sortBy.Descending(dbSortField);
                    }
                    break;
                default:
                    sortBy = sortBy.Ascending(dbSortField);
                    break;
            }
            return sortBy;
        }

        //private static SortByBuilder CreateSortingBuilder(string sortDirection, string dbSortBy)
        //{
        //    SortByBuilder sortBy;
        //    switch (sortDirection)
        //    {
        //        case "+":
        //            {
        //                sortBy = SortBy.Ascending(dbSortBy);
        //            }
        //            break;
        //        case "-":
        //            {
        //                sortBy = SortBy.Descending(dbSortBy);
        //            }
        //            break;
        //        default:
        //            sortBy = SortBy.Ascending(dbSortBy);
        //            break;
        //    }
        //    return sortBy;
        //}

        private static string GetSortByDbFieldName(string sortByProp, Type entityType)
        {
            //orderByProp - expecting a property name of METoDo note: its case sensitive.
            //entityType - typeof( any of our mongo entity classes ) example: typeof( METoDo )
            //return a string of the mongo db field name that is mapped to the given property name:
            //  example: GetOrderByDbFieldName("Description", typeof(METoDo) ) => will return "desc"

            string result = null;
            if (sortByProp != null && sortByProp.ToString().Length > 0)
            {
                PropertyInfo actualProperty = entityType.GetProperty(sortByProp);
                if (actualProperty != null)
                {

                    if (!sortByProp.EndsWith("Property"))
                    {
                        sortByProp += "Property";
                    }
                    FieldInfo dbField = entityType.GetField(sortByProp);
                    if (dbField == null)
                    {
                        throw new ArgumentException("could not find the Sort property: " + sortByProp + " in the Mongo entity " + entityType.Name);
                    }
                    else
                    {
                        if (actualProperty.PropertyType == typeof(string))
                        {
                            //the sorting field is string, try get the lower case property field name (Lowered<property>):                            
                            FieldInfo lowered = entityType.GetField("Lowered" + sortByProp);
                            if (lowered == null)
                            {
                                //sorting will be case sensitive. this is usually not the expected sort for text fields! 
                                //for case insensitive sort - add a Lowered<PropertyName> for lowercase version of this column to the mongo entity.
                                // see METoDo->Title / LoweredTitle as an example.
                            }
                            else
                            {
                                dbField = lowered;
                            }
                        }
                        result = (string)dbField.GetRawConstantValue();
                    }
                }
                else
                {
                    throw new ArgumentException("could not find the Sort property: " + sortByProp + " in the Mongo entity " + entityType.Name);
                }
            }
            return result;
        }
    }
}
