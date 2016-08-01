using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using Phytel.API.DataDomain.Contact.DTO;

namespace Phytel.API.DataDomain.Contact.MongoDB.Repository
{
    public static class PhoneVisitor
    {
        public static void GetContactPhones(List<PhoneData> phoneData, ref MEContact meContact)
        {
            try
            {
                if (phoneData != null)
                {
                    List<Phone> mePhones = phoneData.Select(p => new Phone
                    {
                        Id = ObjectId.GenerateNewId(),
                        Number = p.Number,
                        ExtNumber = p.ExtNumber,
                        IsText = p.IsText,
                        TypeId = ObjectId.Parse(p.TypeId),
                        PreferredPhone = p.PhonePreferred,
                        PreferredText = p.TextPreferred,
                        OptOut = p.OptOut,
                        DeleteFlag = false,
                        DataSource = p.DataSource,
                        ExternalRecordId = p.ExternalRecordId
                    }).ToList();

                    meContact.Phones = mePhones;
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("DD:PhoneVisitor:GetContactPhones " + ex.StackTrace);
            }
        }
    }
}
