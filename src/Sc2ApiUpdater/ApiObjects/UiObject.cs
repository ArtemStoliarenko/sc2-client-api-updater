using System.Runtime.Serialization;

namespace Sc2ApiUpdater
{
    [DataContract]
    public sealed class UiObject : ApiObject
    {
        [DataMember(IsRequired = true, Name = "activeScreens")]
        public string[] ActiveScreens { get; private set; }

        public bool Equals(UiObject uiObjectOther) =>
            (uiObjectOther != null) ? CompareEnumerables(ActiveScreens, uiObjectOther.ActiveScreens) : false;

        public override bool Equals(object obj) => this.Equals(obj as UiObject);

        public override int GetHashCode() => EnumberableHashCode(ActiveScreens);
    }
}
