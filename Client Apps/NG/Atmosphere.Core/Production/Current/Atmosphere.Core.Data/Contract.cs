using System;
using System.Data;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using Phytel.Framework.SQL.Data;
using Phytel.Framework.SQL;

namespace C3.Data
{
    [Serializable]
    [DataContract]
    public class Contract
    {
        [DataMember]
        public int ContractId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Number { get; set; }

        [DataMember]
        public string Server { get; set; }

        [DataMember]
        public string Database { get; set; }

        [DataMember]
        public bool DefaultContract { get; set; }

        [XmlIgnore]
        [IgnoreDataMember]
        public string UserName { get; set; }

        [XmlIgnore]
        [IgnoreDataMember]
        public string Password { get; set; }

        [XmlIgnore]
        [IgnoreDataMember]
        public string ConnectionString { get; set; }

        [DataMember]
        public int PhytelContractId { get; set; }

        [DataMember]
        public bool IsSelected { get; set; }

        public static Contract Build(ITypeReader reader)
        {
            Contract contract = new Contract();

            PhytelEncrypter phytelEncrypter = new PhytelEncrypter();

            contract.ContractId = reader.GetInt("ContractId");
            contract.Name = reader.GetString("Name");
            contract.Number = reader.GetString("Number");
            contract.Database = reader.GetString("Database");
            contract.Server = reader.GetString("Server");
            contract.DefaultContract = reader.GetBool("DefaultContract");
            contract.UserName = reader.GetString("UserName");
            contract.Password = phytelEncrypter.Decrypt(reader.GetString("Password"));
            contract.ConnectionString = string.Format("Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3}", contract.Server, contract.Database, contract.UserName, contract.Password);
            contract.PhytelContractId = reader.GetInt("PhytelContractId");

            return contract;
        }

        //FF: This was done for Impersonation User
        public static Contract BuildLight(DataRow row)
        {
            Contract contract = new Contract();

            contract.ContractId = Converter.ToInt(row["ContractId"].ToString());
            contract.Name = row["Name"].ToString();
            

            return contract;
		}

		#region Contract Property
		//TODO:  Retrieve the ContractProperty table and return it
		#endregion
	}
}
