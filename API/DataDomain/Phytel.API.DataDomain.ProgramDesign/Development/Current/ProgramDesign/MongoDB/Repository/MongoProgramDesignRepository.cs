using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Phytel.API.Common;
using Phytel.API.DataAudit;
using Phytel.API.DataDomain.ProgramDesign;
using Phytel.API.DataDomain.ProgramDesign.DTO;
using Phytel.API.DataDomain.ProgramDesign.MongoDB.DTO;
using Phytel.API.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MB = MongoDB.Driver.Builders;

namespace Phytel.API.DataDomain.ProgramDesign
{
    public class MongoProgramDesignRepository<T> : IProgramDesignRepository<T>
    {
        private string _dbName = string.Empty;

        public MongoProgramDesignRepository(string contractDBName)
        {
            _dbName = contractDBName;
        }

        public string UserId { get; set; }

        public object Insert(object newEntity)
        {
            throw new NotImplementedException();
        }

        public object Insert(object newEntity, string type)
        {
            object result = null;
            switch(type)
            {
                case "module":
                     result = AddModuleToProgram(newEntity);
                     break;
                case "action":
                    result = AddActionToModule(newEntity);
                    break;
                case "step":
                    result = AddStepToAction(newEntity);
                    break;
                case "text":
                     result = AddTextStepToAction(newEntity);
                     break;
                case "yesno":
                     result = AddYesNoStepToAction(newEntity);
                     break;
                default:
                    throw new ArgumentException("Insert method not found: MongoProgramDesignRepository");
            }
            return result;
        }

        public object AddModuleToProgram(object newEntity)
        {
            PutModuleInProgramRequest request = newEntity as PutModuleInProgramRequest;

            MEProgram program = null;
            MEModule module = null;
            using (ProgramDesignMongoContext ctx = new ProgramDesignMongoContext(_dbName))
            {
                //Find program
                var pUQuery = new QueryDocument(MEProgram.IdProperty, ObjectId.Parse(request.ProgramId));
                program = ctx.Programs.Collection.FindOneAs<MEProgram>(pUQuery);

                MongoProgramDesignRepository<T> repo = new MongoProgramDesignRepository<T>(_dbName);
                repo.UserId = this.UserId;

                //Find module
                var mUQuery = new QueryDocument(MEModule.IdProperty, ObjectId.Parse(request.ModuleId));
                module = ctx.Modules.Collection.FindOneAs<MEModule>(mUQuery);

                if (program != null && module != null)
                {
                    List<MEModule> list = new List<MEModule>();
                    if (program.Modules != null)
                    {
                        list = program.Modules;
                    }
                    list.Add(module);
                    program.Modules = list;
                    ctx.Programs.Collection.Update(pUQuery, MB.Update.SetWrapped<List<MEModule>>(MEProgram.ModulesProperty, program.Modules));
                }
                else if (program == null)
                    throw new ArgumentException("Program requested is missing from the DataDomain.");
                else if (module == null)
                    throw new ArgumentException("Module requested is missing from the DataDomain.");
            }

            return new PutModuleInProgramResponse
            {
                Id = program.Id.ToString()
            };
        }

        public object AddActionToModule(object newEntity)
        {
            PutActionInModuleRequest request = newEntity as PutActionInModuleRequest;
            
            MEModule module = null;
            MEAction action = null;
            using (ProgramDesignMongoContext ctx = new ProgramDesignMongoContext(_dbName))
            {
                var mUQuery = new QueryDocument(MEModule.IdProperty, ObjectId.Parse(request.ModuleId));
                module = ctx.Modules.Collection.FindOneAs<MEModule>(mUQuery);

                MongoProgramDesignRepository<T> repo = new MongoProgramDesignRepository<T>(_dbName);
                repo.UserId = this.UserId;

                //Find action
                var aUQuery = new QueryDocument(MEAction.IdProperty, ObjectId.Parse(request.ActionId));
                action = ctx.Actions.Collection.FindOneAs<MEAction>(aUQuery);

                if (module != null && action != null)
                {
                    List<MEAction> list = new List<MEAction>();
                    if (module.Actions != null)
                    {
                        list = module.Actions;
                    }
                    list.Add(action);
                    module.Actions = list;
                    ctx.Modules.Collection.Update(mUQuery, MB.Update.SetWrapped<List<MEAction>>(MEModule.ActionsProperty, module.Actions));
                }
                else if (module == null)
                    throw new ArgumentException("Module requested is missing from the DataDomain.");
                else if (action == null)
                    throw new ArgumentException("Action requested is missing from the DataDomain.");
            }

            return new PutActionInModuleResponse
            {
                Id = module.Id.ToString()
            };
        }

        public object AddStepToAction(object newEntity)
        {
            PutStepInActionRequest request = newEntity as PutStepInActionRequest;

            MEAction action = null;
            MEStep step = null;
            using (ProgramDesignMongoContext ctx = new ProgramDesignMongoContext(_dbName))
            {
                var aUQuery = new QueryDocument(MEAction.IdProperty, ObjectId.Parse(request.ActionId));
                action = ctx.Actions.Collection.FindOneAs<MEAction>(aUQuery);

                MongoProgramDesignRepository<T> repo = new MongoProgramDesignRepository<T>(_dbName);
                repo.UserId = this.UserId;

                //Find step
                var tUQuery = new QueryDocument(MEStep.IdProperty, ObjectId.Parse(request.StepId));
                step = ctx.Steps.Collection.FindOneAs<MEStep>(tUQuery);

                if (action != null && step != null)
                {
                    List<MEStep> list = new List<MEStep>();
                    if (action.Steps != null)
                    {
                        list = action.Steps;
                    }
                    list.Add(step);
                    action.Steps = list;
                    ctx.Actions.Collection.Update(aUQuery, MB.Update.SetWrapped<List<MEStep>>(MEAction.StepsProperty, action.Steps));
                }
                else if (action == null)
                    throw new ArgumentException("Action requested is missing from the DataDomain.");
                else if (step == null)
                    throw new ArgumentException("Step requested is missing from the DataDomain.");
            }

            return new PutStepInActionResponse
            {
                Id = action.Id.ToString()
            };
        }

        public object AddTextStepToAction(object newEntity)
        {
            PutTextStepInActionRequest request = newEntity as PutTextStepInActionRequest;

            MEAction action = null;
            MEStep step = null;
            using (ProgramDesignMongoContext ctx = new ProgramDesignMongoContext(_dbName))
            {
                var aUQuery = new QueryDocument(MEAction.IdProperty, ObjectId.Parse(request.ActionId));
                action = ctx.Actions.Collection.FindOneAs<MEAction>(aUQuery);

                MongoProgramDesignRepository<T> repo = new MongoProgramDesignRepository<T>(_dbName);
                repo.UserId = this.UserId;

                //Find step
                var tUQuery = new QueryDocument(MEStep.IdProperty, ObjectId.Parse(request.StepId));
                step = ctx.Steps.Collection.FindOneAs<MEStep>(tUQuery);

                if (action != null && step != null)
                {
                    List<MEStep> list = new List<MEStep>();
                    if (action.Steps != null)
                    {
                        list = action.Steps;
                    }
                    list.Add(step);
                    action.Steps = list;
                    ctx.Actions.Collection.Update(aUQuery, MB.Update.SetWrapped<List<MEStep>>(MEAction.StepsProperty, action.Steps));
                }
                else if (action == null)
                    throw new ArgumentException("Action requested is missing from the DataDomain.");
                else if (step == null)
                    throw new ArgumentException("Step requested is missing from the DataDomain.");
            }

            return new PutTextStepInActionResponse
            {
                Id = action.Id.ToString()
            };
        }

        public object AddYesNoStepToAction(object newEntity)
        {
            PutYesNoStepInActionRequest request = newEntity as PutYesNoStepInActionRequest;

            MEAction action = null;
            MEStep step = null;
            using (ProgramDesignMongoContext ctx = new ProgramDesignMongoContext(_dbName))
            {
                var aUQuery = new QueryDocument(MEAction.IdProperty, ObjectId.Parse(request.ActionId));
                action = ctx.Actions.Collection.FindOneAs<MEAction>(aUQuery);

                MongoProgramDesignRepository<T> repo = new MongoProgramDesignRepository<T>(_dbName);
                repo.UserId = this.UserId;

                //Find step
                var yUQuery = new QueryDocument(MEStep.IdProperty, ObjectId.Parse(request.StepId));
                step = ctx.Steps.Collection.FindOneAs<MEStep>(yUQuery);

                if (action != null && step != null)
                {
                    List<MEStep> list = new List<MEStep>();
                    if (action.Steps != null)
                    {
                        list = action.Steps;
                    }
                    list.Add(step);
                    action.Steps = list;
                    ctx.Actions.Collection.Update(aUQuery, MB.Update.SetWrapped<List<MEStep>>(MEAction.StepsProperty, action.Steps));
                }
                else if (action == null)
                    throw new ArgumentException("Action requested is missing from the DataDomain.");
                else if (step == null)
                    throw new ArgumentException("Step requested is missing from the DataDomain.");
            }

            return new PutTextStepInActionResponse
            {
                Id = action.Id.ToString()
            };
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
            throw new NotImplementedException();
            //try
            //{
            //    MEProgram cp = null;
            //    using (ProgramDesignMongoContext ctx = new ProgramDesignMongoContext(_dbName))
            //    {
            //        var findcp = MB.Query<MEProgram>.EQ(b => b.Id, ObjectId.Parse(entityID));
            //        cp = ctx.Programs.Collection.Find(findcp).FirstOrDefault();
            //    }
            //    return cp;
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception("DD:Program:FindByID()::" + ex.Message, ex.InnerException);
            //}
        }

        public object FindByName(string entityName)
        {
            throw new NotImplementedException();
            //try
            //{
            //    DTO.Program result = null;

            //    using (ProgramDesignMongoContext ctx = new ProgramDesignMongoContext(_dbName))
            //    {
            //        var findcp = MB.Query<MEProgram>.EQ(b => b.Name, entityName);
            //        MEProgram cp = ctx.Programs.Collection.Find(findcp).FirstOrDefault();

            //        if (cp != null)
            //        {
            //            result = new DTO.Program
            //            {
            //                ProgramID = cp.Id.ToString()
            //            };
            //        }
            //        else
            //        {
            //            throw new ArgumentException("ProgramName is not valid or is missing from the records.");
            //        }
            //    }
            //    return result;
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception("DD:MongoProgramDesignRepository:FindByName()::" + ex.Message, ex.InnerException);
            //}
        }

        public List<ProgramInfo> GetActiveProgramsInfoList(GetAllActiveProgramsRequest request)
        {
            throw new NotImplementedException();
        }

        public Tuple<string, IEnumerable<object>> Select(Interface.APIExpression expression)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> SelectAll()
        {
            throw new NotImplementedException();
        }

        public object Update(object entity)
        {
            throw new NotImplementedException();
        }

        public void CacheByID(List<string> entityIDs)
        {
            throw new NotImplementedException();
        }
    }
}
