using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Common;
using ServiceStack.FluentValidation;
using System.Text.RegularExpressions;

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
            RuleFor(r => r.LastFourSSN).Length(4).Matches("^[0-9]*$").When(r => r.LastFourSSN.IsNullOrEmpty() == false);
            RuleFor(r => r.Gender).Length(1).Matches("^[M,F,O,m,f,o]*$").When(r=>r.Gender.IsNullOrEmpty() == false);
            RuleFor(r => r.MaritalStatusId).Length(1, 2).Matches("^[W|LS|LP|U]*$").When(r =>r.MaritalStatusId.IsNullOrEmpty() == false);
            RuleFor(r => r.ClinicalBackground)
                .Must(ValidatePCPinfo)
                .WithMessage("ClinicalBackground is invalid")
                .When(r => r.ClinicalBackground.IsNullOrEmpty() == false);

        }

        private bool ValidatePCPinfo(string pcpInfo)
        {
            String[] PCPstrings;
            char[] charSeparators = new char[] { ';' };
            PCPstrings = pcpInfo.Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);
            Regex rgx = new Regex(@"^[^|]*[|]{1}[0-9]{10}[|]{1}[^|]*$");
            foreach(string s in PCPstrings)
                if(!rgx.IsMatch(s)) return false;
            return true;

        

        }
        
        
    }
}
