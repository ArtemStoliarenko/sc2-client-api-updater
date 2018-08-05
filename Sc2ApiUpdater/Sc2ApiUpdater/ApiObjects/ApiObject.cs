using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Sc2ApiUpdater
{
    [DataContract]
    public abstract class ApiObject
    {
        internal ApiObject()
        {
        }

        public abstract override bool Equals(object obj);

        public abstract override int GetHashCode();

        protected static bool CompareEnumerables<T>(IEnumerable<T> first, IEnumerable<T> second)
        {
            return (first != null && second != null) ?
                new HashSet<T>(first).SetEquals(new HashSet<T>(second)) :
                first == second;
        }

        protected static int EnumberableHashCode<T>(IEnumerable<T> enumerable)
        {
            return enumerable.Aggregate(0, (result, item) => result ^ item.GetHashCode());
        }
    }
}
