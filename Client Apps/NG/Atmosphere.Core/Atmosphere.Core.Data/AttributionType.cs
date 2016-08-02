using System;
using System.Data;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Phytel.Framework.SQL.Data;

namespace C3.Data
{
    [Serializable]
    [DataContract(Name = "AttributionType", Namespace = "http://www.phytel.com/DataContracts/v1.0")]
    public class AttributionType
    {
        #region Public Properties

        public struct Columns
        {
            public const string ATTRIBUTIONTYPEID = "AttributionTypeId";
            public const string ATTRIBUTIONTYPENAME = "AttributionTypeName";
        }

        [DataMember]
        public bool IsSelected { get; set; }
        [DataMember]
        public int AttributionTypeId { get; set; }
        [DataMember]
        public string AttributionTypeName { get; set; }

        #endregion

        #region Public Methods

        public static AttributionType Build(ITypeReader reader)
        {
            AttributionType attributionType = new AttributionType();

            attributionType.AttributionTypeId = reader.GetInt(Columns.ATTRIBUTIONTYPEID);
            attributionType.AttributionTypeName = reader.GetString(Columns.ATTRIBUTIONTYPENAME);

            return attributionType;
        }

        public static AttributionType Build(DataRow row)
        {
            AttributionType attributionType = new AttributionType();

            attributionType.AttributionTypeId = Convert.ToInt32(row[Columns.ATTRIBUTIONTYPEID].ToString());
            attributionType.AttributionTypeName = row[Columns.ATTRIBUTIONTYPENAME].ToString();
            attributionType.IsSelected = false;

            return attributionType;
        }
        #endregion
    }
}
