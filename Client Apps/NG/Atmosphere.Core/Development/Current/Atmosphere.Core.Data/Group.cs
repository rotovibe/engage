using System;
using System.Data;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Phytel.Framework.SQL.Data;

namespace C3.Data
{
    [Serializable]
    [DataContract(Name = "Group", Namespace = "http://www.phytel.com/DataContracts/v1.0")]
    public class Group
    {   
        #region Constants
        public struct Columns
        {
            public const string GROUPID = "GroupId";
            public const string NAME = "Name"; //TODO: Need to make sure other procs use Groupname this is here for legacy purposes
            public const string GROUPNAME = "GroupName";
            public const string ENABLEALL = "EnableAll";
            public const string CREATEDATE = "CreateDate";
        }
        #endregion

        #region Public Properties

        [DataMember]
        public bool IsSelected { get; set; }
        [DataMember]
        public int GroupId { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public bool EnableAll { get; set; }
        [DataMember]
        public DateTime? CreateDate { get; set; }
       
        //TODO: Add List of Subscribers
        [DataMember]
        public List<Subscriber> Subscribers { get; set; }


        #endregion

        #region Public Methods

        
        public static Group Build(ITypeReader reader)
        {
            Group group = new Group();

            group.GroupId = reader.GetInt(Columns.GROUPID);
            group.Name = reader.GetString(Columns.NAME);
            group.EnableAll = reader.GetBool(Columns.ENABLEALL);
            group.CreateDate = reader.GetDate(Columns.CREATEDATE);

            return group;
        }

        
        public static Group Build(DataRow row)
        {
            Group group = new Group();

            group.GroupId = Convert.ToInt32(row[Columns.GROUPID].ToString());
            group.Name = row[Columns.GROUPNAME].ToString();

            return group;
            
        }

        public static Group BuildLight(DataRow row)
        {
            Group group = new Group();

            group.GroupId = Convert.ToInt32(row[Columns.GROUPID].ToString());

            return group;

        }


        #endregion

        #region Overrides

        public override bool Equals(object obj)
        {
            if(!(obj is Group))
            {
                return false;
            }
            return (obj as Group).GroupId == this.GroupId;
        }

        public override int GetHashCode()
        {
            return this.GroupId;
        }

        #endregion
    }
}
