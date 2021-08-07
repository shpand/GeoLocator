using System.Runtime.InteropServices;

namespace GeoLocator.Models
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct IpRange
    {
        public uint From;
        public uint To;
        public uint LocationIndex;
    }
}