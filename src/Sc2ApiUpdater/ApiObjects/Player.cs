using System;
using System.Runtime.Serialization;

namespace Sc2ApiUpdater
{
    [DataContract]
    public sealed class Player : ApiObject
    {
        private const string randomRaceName = "random";

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
            if (playerOther == null)
                return false;

            return Name.Equals(playerOther.Name, StringComparison.Ordinal) &&
                Type.Equals(playerOther.Type, StringComparison.OrdinalIgnoreCase) &&
                CompareRaces(playerOther.Race) &&
                Result.Equals(playerOther.Result, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj) => this.Equals(obj as Player);

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        private bool CompareRaces(string otherRace)
        {
            if (this.Race != null)
            {
                return this.Race.Equals(otherRace, StringComparison.OrdinalIgnoreCase) ||
                    this.Race.Equals(randomRaceName, StringComparison.OrdinalIgnoreCase);
            }
            else
            {
                return this.Race == otherRace;
            }
        }
    }
}
