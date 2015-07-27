using System;
using System.Collections;

namespace Phytel.Services.SQLServer
{
    [Serializable]
    public class ParameterCollection : CollectionBase
    {
        public ParameterCollection(){}

        public void Add(Parameter newParameter)
        {
            List.Add(newParameter);
        }

        public void Remove(int index)
        {
            if (index > Count - 1 || index < 0)
                throw new Exception("Invalid Index!");
            else
                List.RemoveAt(index);
        }

        public Parameter Item(int Index)
        {
            // The appropriate item is retrieved from the List object and
            // explicitly cast to the Widget type, then returned to the 
            // caller.
            return (Parameter)List[Index];
        }
    }
}