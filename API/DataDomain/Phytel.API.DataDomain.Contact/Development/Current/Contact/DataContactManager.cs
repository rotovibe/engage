using System;
using System.Collections.Generic;
using System.Linq;
using Phytel.API.DataDomain.Contact.DTO;
using Phytel.API.Interface;

namespace Phytel.API.DataDomain.Contact
{
    public static class ContactDataManager
    {
        public static ContactData GetContactByPatientId(GetContactDataRequest request)
        {
            ContactData result = null;
            try
            {
                IContactRepository<GetContactDataResponse> repo = ContactRepositoryFactory<GetContactDataResponse>.GetContactRepository(request.ContractNumber, request.Context);
                result = repo.FindContactByPatientId(request) as ContactData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public static SearchContactsDataResponse SearchContacts(SearchContactsDataRequest request)
        {
            SearchContactsDataResponse response = null;
            try
            {
                IContactRepository<GetContactDataResponse> repo = ContactRepositoryFactory<GetContactDataResponse>.GetContactRepository(request.ContractNumber, request.Context);
                ICollection<SelectExpression> selectExpressions = new List<SelectExpression>();

                //List of contact ids
                if (request.ContactIds != null)
                {
                    SelectExpression contactIDsSelectExpression = new SelectExpression();
                    contactIDsSelectExpression.FieldName = MEContact.IdProperty;
                    contactIDsSelectExpression.Type = SelectExpressionType.IN;
                    contactIDsSelectExpression.Value = request.ContactIds;
                    contactIDsSelectExpression.NextExpressionType = SelectExpressionGroupType.AND;
                    contactIDsSelectExpression.ExpressionOrder = 1;
                    contactIDsSelectExpression.GroupID = 1;
                    selectExpressions.Add(contactIDsSelectExpression);
                }
                // DeleteFlag = false.
                SelectExpression deleteFlagSelectExpression = new SelectExpression();
                deleteFlagSelectExpression.FieldName = MEContact.DeleteFlagProperty;
                deleteFlagSelectExpression.Type = SelectExpressionType.EQ;
                deleteFlagSelectExpression.Value = false;
                deleteFlagSelectExpression.ExpressionOrder = 2;
                deleteFlagSelectExpression.GroupID = 1;
                selectExpressions.Add(deleteFlagSelectExpression);

                APIExpression apiExpression = new APIExpression();
                apiExpression.Expressions = selectExpressions;

                Tuple<string, IEnumerable<object>> contacts = repo.Select(apiExpression);

                if (contacts != null)
                {
                    response = new SearchContactsDataResponse();
                    response.Contacts = contacts.Item2.Cast<ContactData>().ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        public static PutContactDataResponse InsertContact(PutContactDataRequest request)
        {
            PutContactDataResponse response = null;
            try
            {
                IContactRepository<PutContactDataResponse> repo = ContactRepositoryFactory<PutContactDataResponse>.GetContactRepository(request.ContractNumber, request.Context);
                response = repo.Insert(request) as PutContactDataResponse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        public static PutUpdateContactDataResponse UpdateContact(PutUpdateContactDataRequest request)
        {
            PutUpdateContactDataResponse response = null;
            try
            {
                IContactRepository<PutUpdateContactDataResponse> repo = ContactRepositoryFactory<PutUpdateContactDataResponse>.GetContactRepository(request.ContractNumber, request.Context);
                response = repo.Update(request) as PutUpdateContactDataResponse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }
    }
}   
