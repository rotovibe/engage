using System;

namespace Phytel.Mongo.Linq
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=true, Inherited= false)]
    public class MongoIndexAttribute : Attribute
    {
        public bool Descending { get; set; }

        public string[] Keys { get; set; }

        public bool Unique { get; set; }

        /// <summary>
        /// Time to live in seconds
        /// </summary>
        private int timeToLive = -1;

        public int TimeToLive 
        {
            get
            {
                return timeToLive;
            }
            
            set
            {
                timeToLive = value;
            }
        }
    }
}
