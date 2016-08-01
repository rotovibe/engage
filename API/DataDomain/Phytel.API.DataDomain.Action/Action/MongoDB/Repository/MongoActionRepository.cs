using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.Action.DTO;
using Phytel.API.Interface;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Bson;
using Phytel.API.DataDomain.Action;

namespace Phytel.API.DataDomain.Action
{
    public class MongoActionRepository<T> : IActionRepository<T>
    {
        private string _dbName = string.Empty;

        static MongoActionRepository()
        {
            #region Register ClassMap
            try
            {
                if (MongoDB.Bson.Serialization.BsonClassMap.IsClassMapRegistered(typeof(MEAction)) == false)
                    MongoDB.Bson.Serialization.BsonClassMap.RegisterClassMap<MEAction>();
            }
            catch { }
            
            #endregion
        }

        public MongoActionRepository(string contractDBName)
        {
            _dbName = contractDBName;
        }

        public object Insert(object newEntity)
        {
            throw new NotImplementedException();
        }

        public object InsertAll(List<object> entities)
        {
            throw new NotImplementedException();
        }

        public void Delete(object entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(List<object> entities)
        {
            throw new NotImplementedException();
        }

        public object FindByID(string entityID)
        {
            GetActionDataResponse actionResponse = null;
            using (ActionMongoContext ctx = new ActionMongoContext(_dbName))
            {
                List<IMongoQuery> queries = new List<IMongoQuery>();
                queries.Add(Query.EQ(MEAction.IdProperty, ObjectId.Parse(entityID)));
                queries.Add(Query.EQ(MEAction.DeleteFlagProperty, false));
                IMongoQuery mQuery = Query.And(queries);
                MEAction meAction = ctx.Actions.Collection.Find(mQuery).FirstOrDefault();
                if (meAction != null)
                {
                    actionResponse = new GetActionDataResponse();

                    List<string> objectiveIDs = new List<string>();

                    if (meAction.Objectives != null)
                    {
                        foreach (Objective oi in meAction.Objectives)
                        {
                            objectiveIDs.Add(oi.Id.ToString());
                        }
                    }
                    
                    API.DataDomain.Action.DTO.ActionData action = new API.DataDomain.Action.DTO.ActionData
                    {
                        ID = meAction.Id.ToString(),
                        Name = meAction.Name,
                        Description = meAction.Description,
                        CompletedBy = meAction.CompletedBy.ToString(),
                        Objectives = objectiveIDs,
                        Status = toFriendlyString(meAction.Status)
                    };
                    actionResponse.Action = action;
                }
            }
            return actionResponse;
        }

        /// <summary>
        /// Converts the name of the Enum to a friendly name that can be shown on the UI
        /// </summary>
        /// <param name="s">Status enum object</param>
        /// <returns>friendly string</returns>
        private string toFriendlyString(Status s)
        {
            switch (s)
            {
                case Status.Active:
                    return "Active";
                case Status.Inactive:
                    return "Inactive";
                case Status.InReview:
                    return "In Review";
                case Status.Met:
                    return "Met";
                case Status.NotMet:
                    return "Not Met";
            }
            return null;
        }

        public Tuple<string, IEnumerable<object>> Select(Interface.APIExpression expression)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> SelectAll()
        {
            throw new NotImplementedException();
        }

        public GetAllActionsDataResponse SelectAll(double versionNumber, Status status)
        {
            GetAllActionsDataResponse response = new GetAllActionsDataResponse()
            {
                Version = versionNumber
            };

            List<DTO.ActionData> list = new List<DTO.ActionData>();

            using (ActionMongoContext ctx = new ActionMongoContext(_dbName))
            {
                 //var x = (from a in ctx.Actions
                 //       //where a.CompletedBy.ToString().Length > 5
                 //       select new DTO.ActionData
                 //       {
                 //           ID = a.Id.ToString(),
                 //           Name = a.Name,
                 //           Description = a.Description
                 //           //Status = status.ToString()
                 //       });


                var l = from a in ctx.Actions
                        select a;

                 //foreach (ActionData act in x)
                 //{
                 //    if(act.CompletedBy.ToString().Length > 5)
                 //       response.Actions.Add(act);
                 //}

            }
           
            return response;
        }

        public object Update(object entity)
        {
            throw new NotImplementedException();
        }

        public void CacheByID(List<string> entityIDs)
        {
            throw new NotImplementedException();
        }

        public string UserId { get; set; }


        public void UndoDelete(object entity)
        {
            throw new NotImplementedException();
        }
    }
}
