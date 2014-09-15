using System.Collections.Generic;
using System.Linq;
using DataDomain.Template.Repo;
using Phytel.API.DataDomain.Template.DTO;
using System;

namespace Phytel.API.DataDomain.Template
{
    public class TemplateDataManager : ITemplateDataManager
    {
        protected readonly IMongoTemplateRepository TemplateRepository;

        public TemplateDataManager(IMongoTemplateRepository repository)
        {
            TemplateRepository = repository;
        }

        public DTO.Template GetTemplateByID(GetTemplateRequest request)
        {
            try
            {
                DTO.Template result = null;
                TemplateRepository.UserId = request.UserId;
                result = TemplateRepository.FindByID(request.TemplateID) as DTO.Template;

                return result;
            }
            catch (Exception ex)
            { 
                throw new Exception("TemplateDD:GetTemplateByID()::" + ex.Message, ex.InnerException); 
            }
        }

        public List<DTO.Template> GetTemplateList(GetAllTemplatesRequest request)
        {
            try
            {
                List<DTO.Template> result = null;
                TemplateRepository.UserId = request.UserId;
                result = TemplateRepository.SelectAll().Cast<DTO.Template>().ToList<DTO.Template>();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("TemplateDD:GetTemplateList()::" + ex.Message, ex.InnerException);
            }
        }
    }
}   
