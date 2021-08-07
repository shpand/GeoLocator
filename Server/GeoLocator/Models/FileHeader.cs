using System.Runtime.InteropServices;

namespace GeoLocator.Models
{
    //use Pack 1 so that .NET does not try to align fields by 8 byte offset (on 64 bit processors)
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct FileHeader
    {
        public int Version;
        public fixed char Name[16];
        public ulong Timestamp;
        public int RecordCount;
        public uint OffsetRanges;
        public uint OffsetCities;
        public uint OffsetLocations;
    }
}