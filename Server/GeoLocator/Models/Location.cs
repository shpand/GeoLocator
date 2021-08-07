using System.Runtime.InteropServices;

namespace GeoLocator.Models
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct Location
    {
        //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public fixed sbyte Country[8];
        //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
        public fixed sbyte Region[12];
        //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
        public fixed sbyte PostalCode[12];
        //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 24)]
        public fixed sbyte City[24];
        //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public fixed sbyte Organization[32];
        public float Latitude;
        public float Longitude;

        public string CountryGet
        {
            get
            {
                fixed(sbyte* ptr = &Country[0])
                {
                    return new string(ptr);
                }
            }
        }

        public string RegionGet
        {
            get
            {
                fixed(sbyte* ptr = &Region[0])
                {
                    return new string(ptr);
                }
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Location2
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string Country;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
        public string Region;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
        public string PostalCode;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 24)]
        public string City;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string Organization;
        public float Latitude;
        public float Longitude;
    }
}