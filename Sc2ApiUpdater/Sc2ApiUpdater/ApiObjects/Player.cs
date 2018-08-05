using System;
using System.Runtime.Serialization;

namespace Sc2ApiUpdater
{
    [DataContract]
    public sealed class Player : ApiObject
    {
        [DataMember(IsRequired = true, Name = "id")]
        public int Id { get; private set; }

        [DataMember(IsRequired = true, Name = "name")]
        public string Name { get; private set; }

        [DataMember(IsRequired = true, Name = "type")]
        public string Type { get; private set; }

        [DataMember(IsRequired = true, Name = "race")]
        public string Race { get; private set; }

        [DataMember(IsRequired = true, Name = "result")]
        public string Result { get; private set; }

        public bool Equals(Player playerOther)
        {
            return Name.Equals(playerOther.Name, StringComparison.Ordinal) &&
                Type.Equals(playerOther.Type, StringComparison.OrdinalIgnoreCase) &&
                Race.Equals(playerOther.Race, StringComparison.OrdinalIgnoreCase) &&
                Result.Equals(playerOther.Result, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            return (obj is Player playerOther) ?
                this.Equals(playerOther) :
                false;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}
