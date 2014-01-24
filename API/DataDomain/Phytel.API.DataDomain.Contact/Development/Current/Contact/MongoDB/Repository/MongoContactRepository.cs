using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Phytel.API.DataDomain.Contact.DTO;
using Phytel.API.Interface;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Bson;
using Phytel.API.AppDomain.Contact;

namespace Phytel.API.DataDomain.Contact
{
    public class MongoContactRepository<T> : IContactRepository<T>
    {
        private string _dbName = string.Empty;

        public MongoContactRepository(string contractDBName)
        {
            _dbName = contractDBName;
        }

        public object Insert(object newEntity)
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception ex) { throw ex; }
        }

        public object InsertAll(List<object> entities)
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception ex) { throw ex; }
        }

        public void Delete(object entity)
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception ex) { throw ex; }
        }

        public void DeleteAll(List<object> entities)
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception ex) { throw ex; }
        }

        public object FindByID(string entityID)
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception ex) { throw ex; }
        }

        public Tuple<string, IEnumerable<object>> Select(Interface.APIExpression expression)
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception ex) { throw ex; }
        }

        public IEnumerable<object> SelectAll()
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception ex) { throw ex; }
        }

        public object Update(object entity)
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception ex) { throw ex; }
        }

        public void CacheByID(List<string> entityIDs)
        {
            try
            {
                throw new NotImplementedException();
                // code here //
            }
            catch (Exception ex) { throw ex; }
        }

        public object FindContactByPatientId(string patientId)
        {
            GetContactDataResponse response = null;
            using (ContactMongoContext ctx = new ContactMongoContext(_dbName))
            {
                List<IMongoQuery> queries = new List<IMongoQuery>();
                queries.Add(Query.EQ(MEContact.PatientIdProperty, patientId));
                queries.Add(Query.EQ(MEContact.DeleteFlagProperty, false));
                IMongoQuery mQuery = Query.And(queries);
                MEContact mc = ctx.Contacts.Collection.Find(mQuery).FirstOrDefault();
                if (mc != null)
                {
                    response = new GetContactDataResponse();
                    ContactData contactData = new ContactData { ContactId = mc.Id.ToString(), PatientId = mc.PatientId.ToString(), UserId = mc.UserId.ToString(), TimeZoneId = mc.TimeZone.ToString() };

                    //Modes
                    //Phones
                    //Texts
                    //Emails
                    //Addresses
                    //WeekDays
                    //TimesOfDaysId
                    //TimeZoneId
                    //Languages

                    response.Contact = contactData;
                }
            }
            return response;
        }
    }
}
