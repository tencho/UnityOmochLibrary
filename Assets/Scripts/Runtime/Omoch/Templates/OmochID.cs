namespace Omoch.Templates
{
    /// <summary>
    /// intをIDとして使う時のひな型
    /// </summary>
    internal readonly struct OmochID
    {
        private readonly int id;
        public OmochID(int id) => this.id = id;
        public override bool Equals(object value) => value is OmochID chamberID && chamberID.id == id;
        public override int GetHashCode() => id;
        public override string ToString() => id.ToString();

        public static explicit operator int(OmochID chamberID) => chamberID.id;
        public static bool operator ==(OmochID a, OmochID b) => a.id == b.id;
        public static bool operator !=(OmochID a, OmochID b) => !(a == b);
        public static bool operator ==(OmochID? a, OmochID? b) => a?.Equals(b) ?? b is null;
        public static bool operator !=(OmochID? a, OmochID? b) => !(a == b);
    }
}
