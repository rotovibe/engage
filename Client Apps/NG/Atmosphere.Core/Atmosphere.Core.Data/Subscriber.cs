using System;
using System.Data;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Phytel.Framework.SQL.Data;

namespace C3.Data
{
    [Serializable]
    [DataContract(Name = "Subscriber", Namespace = "http://www.phytel.com/DataContracts/v1.0")]
    public class Subscriber
    {
        #region Constants
        public struct Columns
        {
            public const string SUBSCRIBERID = "SubscriberId";
            public const string SUBSCRIBERNAME = "SubscriberName";
            public const string NPI = "NPI";
            public const string TIN = "TIN";
        }

        #endregion

        #region Public Properties

        [DataMember]
        public bool IsSelected { get; set; }

        [DataMember]
        public int SubscriberId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string NPI { get; set; }

        [DataMember]
        public string TIN { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public List<Group> Groups { get; set; }

        public Subscriber()
        {
            Groups = new List<Group>();
        }

        #endregion

        //TODO Need to create list of Groups rather than Id

        #region Public Methods

        public static Subscriber Build(ITypeReader reader)
        {
            Subscriber subscriber = new Subscriber();

            subscriber.SubscriberId = reader.GetInt(Columns.SUBSCRIBERID);
            subscriber.Name = reader.GetString(Columns.SUBSCRIBERNAME);
            //subscriber.Status = reader.GetString("Status");
            return subscriber;
        }

        public static Subscriber BuildPQRSsubscriber(ITypeReader reader)
        {
            Subscriber subscriber = Build(reader);
            subscriber.NPI = reader.GetString(Columns.NPI, string.Empty);
            subscriber.TIN = reader.GetString(Columns.TIN, string.Empty);
            return subscriber;
        }

        public static Subscriber Build(DataRow row)
        {
            Subscriber subscriber = new Subscriber();

            subscriber.SubscriberId = Convert.ToInt32(row[Columns.SUBSCRIBERID].ToString());
            subscriber.Name = row[Columns.SUBSCRIBERNAME].ToString();
            subscriber.IsSelected = false;
            //subscriber.Status = reader.GetString("Status");

            return subscriber;
        }

        public override bool Equals(object obj)
        {
            if (obj is Subscriber)
            {
                return this.SubscriberId == ((Subscriber)obj).SubscriberId;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return this.SubscriberId;
        }

        public override string ToString()
        {
            return this.Name;
        }

        #endregion
    }
}
