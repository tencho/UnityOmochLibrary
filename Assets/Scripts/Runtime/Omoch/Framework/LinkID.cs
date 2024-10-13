using System.Collections.Generic;

namespace Omoch.Framework
{
    public readonly struct LinkID
    {
        private readonly int id;
        private static readonly Dictionary<string, LinkID> idMap = new();
        private static int idCounter;
        
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

        private static LinkID GetLinkID(string prefix, string name)
        {
            var key = prefix + name;
            if (idMap.TryGetValue(key, out var value)) return value;

            var linkID = new LinkID(++idCounter, name);
            idMap[key] = linkID;
            return linkID;
        }

        public override bool Equals(object value) => value is LinkID linkID && linkID.id == id;
        public override int GetHashCode() => id;
        public override string ToString() => $"LinkID:{Name}({id.ToString()})";

        public static explicit operator int(LinkID linkID) => linkID.id;
        public static bool operator ==(LinkID a, LinkID b) => a.id == b.id;
        public static bool operator !=(LinkID a, LinkID b) => !(a == b);
        public static bool operator ==(LinkID? a, LinkID? b) => a?.Equals(b) ?? b is null;
        public static bool operator !=(LinkID? a, LinkID? b) => !(a == b);
    }
}