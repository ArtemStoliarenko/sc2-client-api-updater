using System.Runtime.Serialization;

namespace Sc2ApiUpdater
{
    [DataContract]
    public sealed class UiObject : ApiObject
    {
        [DataMember(IsRequired = true, Name = "activeScreens")]
        public string[] ActiveScreens { get; private set; }

        public bool Equals(UiObject uiObjectOther) => CompareEnumerables(ActiveScreens, uiObjectOther.ActiveScreens);

        public override bool Equals(object obj)
        {
            return (obj is UiObject uiObjectOther) ?
                this.Equals(uiObjectOther) :
                false;
        }

        public override int GetHashCode() => EnumberableHashCode(ActiveScreens);
    }
}
