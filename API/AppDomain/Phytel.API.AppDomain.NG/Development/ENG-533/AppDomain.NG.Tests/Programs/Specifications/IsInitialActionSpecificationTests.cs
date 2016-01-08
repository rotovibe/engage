using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.AppDomain.NG.Specifications;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Phytel.API.AppDomain.NG.DTO;
using MongoDB.Bson;
namespace Phytel.API.AppDomain.NG.Specifications.Tests
{
    [TestClass()]
    public class IsInitialActionSpecificationTests
    {
        [TestClass()]
        public class IsSatisfiedBy_Method
        {
            [TestMethod()]
            public void No_Actions_completed()
            {
                Program p = new Program
                {
                    Name = "sample program",
                    Id = ObjectId.GenerateNewId().ToString(),
                    Description = "Sample description",
                    Modules = new List<Module>
                    {
                        new Module{ Id = ObjectId.GenerateNewId().ToString(),
                             Name = "Sample Module 1",
                              Actions = new List<Actions>{
                                  new Actions{
                                       Id = ObjectId.GenerateNewId().ToString(),
                                       Name = "Module 1 action 1",
                                        Completed = false
                                  },
                                  new Actions{
                                      Id = ObjectId.GenerateNewId().ToString(),
                                      Name = "Module 1 action 2",
                                        Completed = false
                                  }
                              }
                        },
                        new Module{ Id = ObjectId.GenerateNewId().ToString(),
                             Name = "Sample Module 2",
                              Actions = new List<Actions>{
                                  new Actions{
                                       Id = ObjectId.GenerateNewId().ToString(),
                                        Name = "Module 2 action 1",
                                        Completed = false
                                  },
                                  new Actions{
                                      Id = ObjectId.GenerateNewId().ToString(),
                                      Name = "Module 2 action 2",
                                        Completed = false
                                  }
                              }
                        }
                    }
                };

                IsInitialActionSpecification<Program> IsInitAction = new IsInitialActionSpecification<Program>();
                bool result = IsInitAction.IsSatisfiedBy(p);

                Assert.IsTrue(result);
            }

            [TestMethod()]
            public void One_Action_completed()
            {
                Program p = new Program
                {
                    Name = "sample program",
                    Id = ObjectId.GenerateNewId().ToString(),
                    Description = "Sample description",
                    Modules = new List<Module>
                    {
                        new Module{ Id = ObjectId.GenerateNewId().ToString(),
                             Name = "Sample Module 1",
                              Actions = new List<Actions>{
                                  new Actions{
                                       Id = ObjectId.GenerateNewId().ToString(),
                                       Name = "Module 1 action 1",
                                        Completed = false
                                  },
                                  new Actions{
                                      Id = ObjectId.GenerateNewId().ToString(),
                                      Name = "Module 1 action 2",
                                        Completed = false
                                  }
                              }
                        },
                        new Module{ Id = ObjectId.GenerateNewId().ToString(),
                             Name = "Sample Module 2",
                              Actions = new List<Actions>{
                                  new Actions{
                                       Id = ObjectId.GenerateNewId().ToString(),
                                        Name = "Module 2 action 1",
                                        Completed = false
                                  },
                                  new Actions{
                                      Id = ObjectId.GenerateNewId().ToString(),
                                      Name = "Module 2 action 2",
                                        Completed = true
                                  }
                              }
                        }
                    }
                };

                IsInitialActionSpecification<Program> IsInitAction = new IsInitialActionSpecification<Program>();
                bool result = IsInitAction.IsSatisfiedBy(p);

                Assert.IsFalse(result);
            }

            [TestMethod()]
            public void Two_Actions_completed_Different_Modules()
            {
                Program p = new Program
                {
                    Name = "sample program",
                    Id = ObjectId.GenerateNewId().ToString(),
                    Description = "Sample description",
                    Modules = new List<Module>
                    {
                        new Module{ Id = ObjectId.GenerateNewId().ToString(),
                             Name = "Sample Module 1",
                              Actions = new List<Actions>{
                                  new Actions{
                                       Id = ObjectId.GenerateNewId().ToString(),
                                       Name = "Module 1 action 1",
                                        Completed = true
                                  },
                                  new Actions{
                                      Id = ObjectId.GenerateNewId().ToString(),
                                      Name = "Module 1 action 2",
                                        Completed = false
                                  }
                              }
                        },
                        new Module{ Id = ObjectId.GenerateNewId().ToString(),
                             Name = "Sample Module 2",
                              Actions = new List<Actions>{
                                  new Actions{
                                       Id = ObjectId.GenerateNewId().ToString(),
                                        Name = "Module 2 action 1",
                                        Completed = false
                                  },
                                  new Actions{
                                      Id = ObjectId.GenerateNewId().ToString(),
                                      Name = "Module 2 action 2",
                                        Completed = true
                                  }
                              }
                        }
                    }
                };

                IsInitialActionSpecification<Program> IsInitAction = new IsInitialActionSpecification<Program>();
                bool result = IsInitAction.IsSatisfiedBy(p);

                Assert.IsFalse(result);
            }
        }
    }
}
