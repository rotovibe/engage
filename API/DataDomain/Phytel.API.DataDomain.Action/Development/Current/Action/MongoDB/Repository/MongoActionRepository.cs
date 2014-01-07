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
using Phytel.API.AppDomain.Action;
using Phytel.API.Common;

namespace Phytel.API.DataDomain.Action
{
    public class MongoActionRepository<T> : IActionRepository<T>
    {
        private string _dbName = string.Empty;

        public MongoActionRepository(string contractDBName)
        {
            _dbName = contractDBName;
        }

        public object Insert(object newEntity)
        {
            throw new NotImplementedException();
        }

        public object InsertAll(List<T> entities)
        {
            throw new NotImplementedException();
        }

        public void Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteAll(List<T> entities)
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

                    if (meAction.ObjectivesInfo != null)
                    {
                        foreach (ObjectiveInfo oi in meAction.ObjectivesInfo)
                        {
                            objectiveIDs.Add(oi.ID.ToString());
                        }
                    }
                    
                    API.DataDomain.Action.DTO.ActionData action = new API.DataDomain.Action.DTO.ActionData
                    {
                        ID = meAction.Id.ToString(),
                        Name = meAction.Name,
                        Description = meAction.Description,
                        CompletedBy = meAction.CompletedBy,
                        Objectives = objectiveIDs,
                        Status = Helper.ToFriendlyString(meAction.Status)
                    };
                    actionResponse.Action = action;
                }
            }
            return actionResponse;
        }

        public Tuple<string, IEnumerable<object>> Select(Interface.APIExpression expression)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> SelectAll()
        {
            throw new NotImplementedException();
        }

        public object Update(T entity)
        {
            throw new NotImplementedException();
        }

        public void CacheByID(List<string> entityIDs)
        {
            throw new NotImplementedException();
        }
    }
}
