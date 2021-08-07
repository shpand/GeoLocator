using System.Runtime.InteropServices;

namespace GeoLocator.Models
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FileHeader2
    {
        public int Version;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string Name;
        public ulong Timestamp;
        public int RecordCount;
        public uint OffsetRanges;
        public uint OffsetCities;
        public uint OffsetLocations;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct FileHeader
    {
        public int Version;
        //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public fixed char Name[16];
        public ulong Timestamp;
        public int RecordCount;
        public uint OffsetRanges;
        public uint OffsetCities;
        public uint OffsetLocations;
    }
}