using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.FluentValidation;
using ServiceStack.Text;

namespace AppDomain.Engage.Population.DTO.Demographics
{
   

    public class PatientValidator : AbstractValidator<Patient>
    {
        public PatientValidator()
        {
            RuleFor(r => r.DataSource).NotEmpty().NotNull();
            RuleFor(r => r.FirstName).NotEmpty().NotNull();
            RuleFor(r => r.LastName).NotEmpty().NotNull();
            
            RuleFor(r => r.DOB).NotEmpty().NotNull();
            RuleFor(r => r.ExternalRecordId).NotEmpty().NotNull();
            RuleFor(r => r.PriorityData).LessThan(4);
            RuleFor(r => r.DeceasedId).LessThan(3);
            RuleFor(r => r.LastFourSSN).Length(4).Matches("^[0-9]*$");
            RuleFor(r => r.Gender).Length(1).Matches("^[M,F,O]$");
            RuleFor(r => r.MaritalStatusId).Length(1,2).Matches("^[W,LF,LS,U]$");

        }

        

        
    }
}
