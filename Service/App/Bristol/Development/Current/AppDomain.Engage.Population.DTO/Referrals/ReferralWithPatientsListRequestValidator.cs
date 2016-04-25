using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppDomain.Engage.Population.DTO.Demographics;
using ServiceStack.FluentValidation;


namespace AppDomain.Engage.Population.DTO.Referrals
{

    public class ReferralWithPatientsListRequestValidator:AbstractValidator<PostReferralWithPatientsListRequest>
    {
        public ReferralWithPatientsListRequestValidator()
        {
            RuleFor(r => r.UserId).NotEmpty().NotNull();
            RuleFor(r => r.Context).NotEmpty().NotNull();
            RuleFor(r => r.ContractNumber).NotEmpty().NotNull();
            RuleFor(r => r.Version).NotEmpty().NotNull();
            RuleFor(r => r.PatientsData).SetCollectionValidator(new PatientValidator());

        }
    }
}
