using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Phytel.API.DataDomain.Contact.DTO;
using Phytel.API.DataDomain.Contact.DTO.ContactTypeLookUp;
using Phytel.API.DataDomain.Contact.MongoDB.DTO;

namespace Phytel.API.DataDomain.Contact.ContactTypeLookUp
{
    public class ContactTypeLookUpManager : IContactTypeLookUpManager
    {
        private readonly IContactTypeLookUpRepositoryFactory _factory;
        public ContactTypeLookUpManager(IContactTypeLookUpRepositoryFactory factory)
        {
            if (factory == null)
                throw new ArgumentNullException("factory");

            _factory = factory;
        }


        #region IContactTypeLookUpManager Members

        public GetContactTypeLookUpDataResponse GetContactTypeLookUps(DTO.GetContactTypeLookUpDataRequest request)
        {
            var response = new GetContactTypeLookUpDataResponse();
            List<MEContactTypeLookup> data = null;

            var contactTypeLookupHierarchy = new List<ContactTypeLookUpData>();

            var repository = _factory.GetContactTypeLookUpRepository(request, RepositoryType.ContactTypeLookUp);

            if (repository == null)
                throw new Exception("The repository is null");

            data = (List<MEContactTypeLookup>)repository.GetContactTypeLookUps((ContactLookUpGroupType)request.GroupType);

            if (request.FlattenData)
            {
                foreach (var flattenedRole in data)
                {

                    var role = new ContactTypeLookUpData
                    {
                        Id = flattenedRole.Id.ToString(),
                        Name = flattenedRole.Name,
                        Role = flattenedRole.Role,
                        Group = (ContactLookUpGroupType) flattenedRole.GroupId,
                        ParentId = flattenedRole.ParentId.ToString()
                    };
                    contactTypeLookupHierarchy.Add(role);
                }


            }
            else
            {
                contactTypeLookupHierarchy = CreateRoleHierarchy(data, "000000000000000000000000");
            }
            
            

            response.ContactTypeLookUps = contactTypeLookupHierarchy;
            response.Version = 1.0;


            return response;
        }

        public PutContactTypeLookUpDataResponse SavContactTypeLookUp(PutContactTypeLookUpDataRequest request)
        {

            var response = new PutContactTypeLookUpDataResponse();

            if(request == null)
                throw new ArgumentNullException("request");

            var repository = _factory.GetContactTypeLookUpRepository(request, RepositoryType.ContactTypeLookUp);

            var dataResponse = repository.SaveContactTypeLookUp(request.ContactTypeLookUpData, repository.UserId);

            response.Id = dataResponse;

            return response;

        }

        #endregion

        #region Private Methods

        private static List<ContactTypeLookUpData> CreateRoleHierarchy(List<MEContactTypeLookup> flattenedRoles, string parentId)
        {
            var docsToAdd = new List<MEContactTypeLookup>(flattenedRoles.Where(r => r.ParentId.ToString() == parentId.ToString()));
            var roleHierarchy = new List<ContactTypeLookUpData>();

            foreach (var flattenedRole in docsToAdd)
            {
                var role = new ContactTypeLookUpData
                {
                    Id = flattenedRole.Id.ToString(),
                    Name = flattenedRole.Name,
                    Role = flattenedRole.Role,
                    Group = (ContactLookUpGroupType)flattenedRole.GroupId,
                    ParentId = flattenedRole.ParentId.ToString()
                };


                if (flattenedRoles.Any(r => r.ParentId.ToString() == flattenedRole.Id.ToString()))
                {
                    role.Children = CreateRoleHierarchy(flattenedRoles, flattenedRole.Id.ToString());
                }


                roleHierarchy.Add(role);

            }

            return roleHierarchy;
        }

        #endregion
        
    }
}
