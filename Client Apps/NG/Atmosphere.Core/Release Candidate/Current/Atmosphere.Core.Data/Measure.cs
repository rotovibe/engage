using Phytel.Framework.SQL;
using System;
using System.Data;
using System.Runtime.Serialization;
using Phytel.Framework.SQL.Data;

namespace C3.Data
{
    [Serializable]
    [DataContract(Name = "Measure", Namespace = "http://www.phytel.com/DataContracts/v1.0")]
    public class Measure
    {
        #region Constants
        public struct Columns
        {
            public const string MEASUREIDCOLUMN = "MeasureId";
            public const string MEASURENAMECOLUMN = "MeasureName";
            public const string NUMERATORCOLUMN = "Numerator";
            public const string DENOMINATORCOLUMN = "Denominator";
            public const string EXCLUDEDCOLUMN = "Excluded";
            public const string MEASUREGOALCOLUMN = "MeasureGoal";
            public const string TOTALPATIENTSCOLUMN = "TotalPatients";
            public const string DENOMINATORID = "DenominatorId";
            public const string LASTVALUE = "LastValue";
            public const string LASTDATE = "LastDate";
            public const string CONDITION = "Condition";
            
        }
        

        #endregion

        #region Public Properties

        [DataMember]
        public bool IsSelected { get; set; }

        [DataMember]
        public int MeasureId { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int Numerator { get; set; }

        [DataMember]
        public int Denominator { get; set; }

        [DataMember]
        public int Excluded { get; set; }      

        [DataMember]
        public double MeasureGoal { get; set; }

        [DataMember]
        public int DenominatorId { get; set; }

        [DataMember]
        public string Percentage { get; set; }

        [DataMember]
        public string LastValue { get; set; }

        [DataMember]
        public DateTime? LastDate { get; set; }

        [DataMember]
        public string Condition { get; set; }

        public override bool Equals(object obj)
        {
            if(obj is Measure)
                return ((Measure) obj).MeasureId == this.MeasureId;
            return false;
        }

        public override int GetHashCode()
        {
            return this.MeasureId;
        }

        public override string ToString()
        {
            return this.Name;
        }

        #endregion

        #region Public Methods
        
        
        public static Measure Build(ITypeReader reader)
        {
            Measure measure = new Measure();

            measure.MeasureId = reader.GetInt(Columns.MEASUREIDCOLUMN);
            measure.Name = reader.GetString(Columns.MEASURENAMECOLUMN);
            measure.Numerator = reader.GetInt(Columns.NUMERATORCOLUMN);
            measure.Denominator = reader.GetInt(Columns.DENOMINATORCOLUMN);
            measure.Excluded = reader.GetInt(Columns.EXCLUDEDCOLUMN);
            measure.LastDate = reader.GetDate(Columns.LASTDATE);
            measure.LastValue = reader.GetString(Columns.LASTVALUE);

            double percentage = ((double)measure.Numerator / (double)measure.Denominator) * (double)100;
            measure.Percentage = String.Format("{0:0 %}", percentage.ToString());

            return measure;
        }

        
        public static Measure Build(DataRow row)
        {
            Measure measure = new Measure();

            measure.MeasureId = Convert.ToInt32(row[Columns.MEASUREIDCOLUMN].ToString());
            measure.Name = row[Columns.MEASURENAMECOLUMN].ToString();
            measure.Numerator = Convert.ToInt32(row[Columns.NUMERATORCOLUMN].ToString());
            measure.Denominator = Convert.ToInt32(row[Columns.DENOMINATORCOLUMN].ToString());
            measure.Excluded = Convert.ToInt32(row[Columns.EXCLUDEDCOLUMN].ToString());
                
            measure.IsSelected = false;

            if (measure.Denominator != 0)
            {
                double percentage = ((double)measure.Numerator / (double)measure.Denominator) * (double)100;
                measure.Percentage = String.Format("{0:0%}", percentage.ToString());
            }
            else
            {
                measure.Percentage = "0.00";
            }

            return measure;
        }

        public static Measure BuildCareOpportunity(DataRow row)
        {
            Measure measure = new Measure();

            measure.MeasureId = Convert.ToInt32(row[Columns.MEASUREIDCOLUMN].ToString());
            measure.Name = row[Columns.MEASURENAMECOLUMN].ToString();
            measure.LastDate = Converter.ToDate(row[Columns.LASTDATE]);
            measure.LastValue = row[Columns.LASTVALUE].ToString();
            measure.Condition = row[Columns.CONDITION].ToString();
            measure.IsSelected = false;

            if (measure.Denominator != 0)
            {
                double percentage = ((double)measure.Numerator / (double)measure.Denominator) * (double)100;
                measure.Percentage = String.Format("{0:0%}", percentage.ToString());
            }
            else
            {
                measure.Percentage = "0.00";
            }

            return measure;
        }

        
        public static Measure BuildLight(DataRow row)
        {
            Measure measure = new Measure();

            measure.MeasureId = Convert.ToInt32(row[Columns.MEASUREIDCOLUMN].ToString());
            measure.DenominatorId = Convert.ToInt32(row[Columns.DENOMINATORID].ToString());
            measure.Name = row[Columns.MEASURENAMECOLUMN].ToString();
            measure.MeasureGoal = Convert.ToDouble(row[Columns.MEASUREGOALCOLUMN].ToString());

            return measure;
        }

        public static Measure BuildBasic(DataRow row)
        {
            Measure measure = new Measure();

            measure.MeasureId = Convert.ToInt32(row[Columns.MEASUREIDCOLUMN].ToString());
            measure.Name = row[Columns.MEASURENAMECOLUMN].ToString();

            return measure;
        }

        public static Measure BuildBasic(ITypeReader reader)
        {
            Measure measure = new Measure();

            measure.MeasureId = reader.GetInt(Columns.MEASUREIDCOLUMN);
            measure.Name = reader.GetString(Columns.MEASURENAMECOLUMN);

            return measure;
        }

        #endregion
    }
}
