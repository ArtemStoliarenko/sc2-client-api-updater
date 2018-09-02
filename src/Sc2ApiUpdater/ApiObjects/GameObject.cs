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
            if (gameObjectOther == null)
                return false;

            return IsReplay == gameObjectOther.IsReplay &&
                gameObjectOther.DisplayTime >= this.DisplayTime &&
                CompareEnumerables(Players, gameObjectOther.Players);
        }

        public override bool Equals(object obj) => this.Equals(obj as GameObject);

        public override int GetHashCode()
        {
            return Convert.ToInt32(IsReplay) ^ EnumberableHashCode(Players);
        }
    }
}
