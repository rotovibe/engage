using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Phytel.Framework.SQL.Data;

namespace C3.Data
{
    [Serializable]
    [DataContract]
    public class SecurityQuestion
    {
        [DataMember]
        public int QuestionId { get; set; }

        [DataMember]
        public string Question { get; set; }

        public static SecurityQuestion Build(ITypeReader reader)
        {
            SecurityQuestion securityQuestion = new SecurityQuestion();
            securityQuestion.QuestionId = reader.GetInt("SecurityQuestionId");
            securityQuestion.Question = reader.GetString("Question");
            return securityQuestion;
        }
    }
}
