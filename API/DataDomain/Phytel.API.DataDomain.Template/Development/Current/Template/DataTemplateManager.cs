using DataDomain.Template.Repo;
using Phytel.API.DataDomain.Template.DTO;
using System.Data.SqlClient;
using Phytel.API.DataDomain.Template;
using System;
using Phytel.API.Common.Format;
using ServiceStack.WebHost.Endpoints;

namespace Phytel.API.DataDomain.Template
{
    public class TemplateDataManager : ITemplateDataManager
    {
        //public IMongoTemplateRepository<TemplateMongoContext> TemplateRepository { get; set; }
        protected readonly IMongoTemplateRepository TemplateRepository;

        public TemplateDataManager(IMongoTemplateRepository repository)
        {
            TemplateRepository = repository;
        }

        public GetTemplateResponse GetTemplateByID(GetTemplateRequest request)
        {
            try
            {
                var result = new GetTemplateResponse();

                //ITemplateRepository repo = TemplateRepositoryFactory.GetTemplateRepository(request, RepositoryType.Template);
                TemplateRepository.UserId = request.UserId;
                result = TemplateRepository.FindByID(request.TemplateID) as GetTemplateResponse;

                return (result ?? new GetTemplateResponse());
            }
            catch (Exception ex)
            { 
                throw new Exception("TemplateDD:GetTemplateByID()::" + ex.Message, ex.InnerException); 
            }
        }

        public GetAllTemplatesResponse GetTemplateList(GetAllTemplatesRequest request)
        {
            try
            {
                var result = new GetAllTemplatesResponse();

                //ITemplateRepository repo = TemplateRepositoryFactory.GetTemplateRepository(request, RepositoryType.Template);
                TemplateRepository.UserId = request.UserId;
                //result = TemplateRepository.SelectAll() as GetAllTemplatesResponse;

                return (result ?? new GetAllTemplatesResponse());
            }
            catch (Exception ex)
            {
                throw new Exception("TemplateDD:GetTemplateList()::" + ex.Message, ex.InnerException);
            }
        }
    }
}   
