using System.Collections.Generic;

namespace Omoch.Framework
{
    public readonly struct LinkID
    {
        private readonly int id;
        public string Name { get; }

        private LinkID(int id, string name)
        {
            this.id = id;
            Name = name;
        }

        public static LinkID From(string stringKey)
        {
            return GetLinkID("s_", stringKey);
        }

        public static LinkID From(int intKey)
        {
            return GetLinkID("i_", intKey.ToString());
        }

        private static readonly Dictionary<string, LinkID> idMap = new();
        private static int idCounter = 0;

        private static LinkID GetLinkID(string prefix, string name)
        {
            var key = prefix + name;
            if (!idMap.ContainsKey(key))
            {
                var linkID = new LinkID(++idCounter, name);
                idMap[key] = linkID;
                return linkID;
            }
            return idMap[key];
        }

        public override bool Equals(object value) => value is LinkID linkID && linkID.id == id;
        public override int GetHashCode() => id;
        public override string ToString() => $"LinkID:{Name}({id.ToString()})";

        public static explicit operator int(LinkID chamberID) => chamberID.id;
        public static bool operator ==(LinkID a, LinkID b) => a.id == b.id;
        public static bool operator !=(LinkID a, LinkID b) => !(a == b);
        public static bool operator ==(LinkID? a, LinkID? b) => a?.Equals(b) ?? b is null;
        public static bool operator !=(LinkID? a, LinkID? b) => !(a == b);
    }
}
