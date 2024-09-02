using System.Collections.Generic;

namespace Omoch.Framework
{
    public readonly struct LinkID
    {
        private readonly int id;
        private LinkID(int id) => this.id = id;

        public static LinkID From(string value)
        {
            return GetLinkID("s_" + value);
        }

        public static LinkID From(int value)
        {
            return GetLinkID("i_" + value.ToString());
        }

        private static readonly Dictionary<string, LinkID> idMap = new();
        private static int idCounter = 0;
        private static LinkID GetLinkID(string key)
        {
            if (!idMap.ContainsKey(key))
            {
                var linkID = new LinkID(++idCounter);
                idMap[key] = linkID;
                return linkID;
            }
            return idMap[key];
        }

        public override bool Equals(object value) => value is LinkID linkID && linkID.id == id;
        public override int GetHashCode() => id;
        public override string ToString() => $"LinkID({id.ToString()})";

        public static explicit operator int(LinkID chamberID) => chamberID.id;
        public static bool operator ==(LinkID a, LinkID b) => a.id == b.id;
        public static bool operator !=(LinkID a, LinkID b) => !(a == b);
        public static bool operator ==(LinkID? a, LinkID? b) => a?.Equals(b) ?? b is null;
        public static bool operator !=(LinkID? a, LinkID? b) => !(a == b);
    }
}
