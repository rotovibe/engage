using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Phytel.Framework.SQL.Data;
using C3.Data;

namespace C3.Business
{
    public class SecurityQuestionService:SqlDataAccessor
    {
        private string _dbConnName = "Phytel";

        public SecurityQuestionService()
        {
            _dbConnName = System.Configuration.ConfigurationManager.AppSettings.Get("PhytelServicesConnName");
        }

        public List<SecurityQuestion> GetAll()
        {
            return CachedQueryAll<SecurityQuestion>(null, _dbConnName, StoredProcedure.GetSecurityQuestion, SecurityQuestion.Build, new CacheAccessor("C3Cache", "SecurityQuestions"));
        }

        public DataTable GetAllDT()
        {
            return Query(null, _dbConnName, StoredProcedure.GetSecurityQuestion);
        }

        public User SetByUser(User user, string question, string answer)
        {
            if (user.UserId != null && !String.IsNullOrEmpty(question) && !String.IsNullOrEmpty(answer))
            {
                try
                {
                    new SqlDataExecutor().Execute(_dbConnName, StoredProcedure.SetSecurityQuestionAndAnswer, user.UserId, question, answer);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return user;
        }
    }
}
