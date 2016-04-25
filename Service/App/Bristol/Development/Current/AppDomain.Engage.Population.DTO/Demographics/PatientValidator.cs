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
            RuleFor(r => r.ExternalRecordId).NotEmpty().NotNull();
            RuleFor(r => r.PriorityData).LessThan(4);
            RuleFor(r => r.DeceasedId).LessThan(2);
            RuleFor(r => r.LastFourSSN).Length(4);
            RuleFor(r => r.FullSsn).Length(8);
            
        }
    }
}
