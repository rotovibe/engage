using System;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace Phytel.Services
{
    public class Parameter
    {
        #region Private Variables
        private string _parameterName;
        private object _parameterValue;
        private SqlDbType _parameterType;
        private ParameterDirection _parameterDirection;
        private int _parameterSize;
        #endregion

        public Parameter(string paramName, object paramValue, SqlDbType paramType, ParameterDirection paramDirection, int paramSize)
        {
            _parameterName = paramName;
            _parameterValue = paramValue;
            _parameterType = paramType;
            _parameterDirection = paramDirection;
            _parameterSize = paramSize;
        }

        #region Properties
        public string Name
        {
            get { return _parameterName; }
            set { _parameterName = value; }
        }

        public object Value
        {
            get { return _parameterValue; }
            set { _parameterValue = value; }
        }

        public SqlDbType Type
        {
            get { return _parameterType; }
            set { _parameterType = value; }
        }

        public ParameterDirection Direction
        {
            get { return _parameterDirection; }
            set { _parameterDirection = value; }
        }

        public int Size
        {
            get { return _parameterSize; }
            set { _parameterSize = value; }
        }
        #endregion

    }
}
