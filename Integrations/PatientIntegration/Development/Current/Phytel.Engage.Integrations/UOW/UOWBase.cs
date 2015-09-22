using System.Collections.Generic;

namespace Phytel.Engage.Integrations.UOW
{
    public abstract class UowBase<T>
    {
        public List<T> Pocos = new List<T>();

        public void Add(T obj)
        {
            if (obj.GetType() == typeof(T))
            {
                Pocos.Add(obj);
            }
        }
    }
}
