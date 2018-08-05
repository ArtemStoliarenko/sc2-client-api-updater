using System;
using System.Runtime.Serialization;

namespace Sc2ApiUpdater
{
    [DataContract]
    public sealed class GameObject : ApiObject
    {
        [DataMember(IsRequired = true, Name = "isReplay")]
        public bool IsReplay { get; private set; }

        [DataMember(IsRequired = true, Name = "displayTime")]
        public double DisplayTime { get; private set; }

        [DataMember(IsRequired = true, Name = "players")]
        public Player[] Players { get; private set; }

        public bool Equals(GameObject gameObjectOther)
        {
            return IsReplay == gameObjectOther.IsReplay &&
                CompareEnumerables(Players, gameObjectOther.Players);
        }

        public override bool Equals(object obj)
        {
            return (obj is GameObject gameObjectOther) ?
                this.Equals(gameObjectOther) :
                false;
        }

        public override int GetHashCode()
        {
            return Convert.ToInt32(IsReplay) ^ EnumberableHashCode(Players);
        }
    }
}
