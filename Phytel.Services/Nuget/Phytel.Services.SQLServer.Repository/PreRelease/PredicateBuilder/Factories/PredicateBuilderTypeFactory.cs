using System;
using System.Collections.Generic;

namespace Phytel.Services.SQLServer.Repository.PredicateBuilder
{
    public class PredicateBuilderTypeFactory
    {
        private Dictionary<TypeCode, IPredicateBuilder> map = new Dictionary<TypeCode, IPredicateBuilder>();

        public IPredicateBuilder GetPredicateBuilderForDataType(TypeCode dataType)
        {
            // Based on the data type for the current DynamicFilter row, this factory will
            // send back an ILambdaBuilder object that handles that data type
            // or a NullLambdaBuilder if no match is found
            CreatePredicateBuilderDictionary();
            if (map.ContainsKey(dataType))
            {
                return map[dataType];
            }
            else
            {
                return new PredicateBuilderNull();
            }
        }

        private void CreatePredicateBuilderDictionary()
        {
            map.Clear();
            Register(TypeCode.String, new PredicateBuilderString());
            Register(TypeCode.Int32, new PredicateBuilderInteger());
            Register(TypeCode.DateTime, new PredicateBuilderDate());
            Register(TypeCode.Boolean, new PredicateBuilderBoolean());
            Register(TypeCode.Decimal, new PredicateBuilderDecimal());
            Register(TypeCode.Double, new PredicateBuilderDecimal());
        }

        private void Register(TypeCode dataType, IPredicateBuilder controlFactoryClass)
        {
            map.Add(dataType, controlFactoryClass);
        }
    }
}
